using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public class LeaveRequest : Entity
{
    private readonly List<EmployeeDocument> _employeeDocuments = [];

    public int Id { get; private set; }

    public Guid RequestingEmployeeId { get; private set; }
    public Employee RequestingEmployee { get; private set; }

    public int LeaveTypeId { get; private set; }
    public LeaveType? LeaveType { get; private set; }

    public DateOnly StartedAt { get; private set; }
    public DateOnly EndedAt { get; private set; }
    public int TotalDays { get; private set; }

    public Guid SubstitutorId { get; private set; }
    public Employee Substitutor { get; private set; }

    public string? Comment { get; private set; }

    public Guid ApproverId { get; private set; }
    public Employee Approver { get; set; }

    public string? ApproverComment { get; private set; }

    public string? ReasonDescription { get; private set; }
    public IReadOnlyList<EmployeeDocument> EmployeeDocuments  => _employeeDocuments.AsReadOnly(); // new

    public RequestStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? DecidedAt { get; private set; }

    private LeaveRequest() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="LeaveRequest"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="leaveRequestRuleSet">Instance of <see cref="ILeaveRequestRuleSet"/> for rules checking</param>
    /// <param name="requestingEmployee">Employee requesting leave</param>
    /// <param name="leaveType">Type of leave choosing by employee</param>
    /// <param name="startedAt">Date of leave starting</param>
    /// <param name="endedAt">Date of leave ending</param>
    /// <param name="approver">Superior approving request</param>
    /// <param name="substitutor">Employee substituting absent Employee</param>
    /// <param name="comment">Optional. Comment of requesting employee for leave request</param>
    /// <param name="approverComment">Optional. Comment of approver for leave request</param>
    /// <param name="reasonDescription">Optional. Description of reason of taking leave</param>
    /// <param name="employeeDocuments">Optional. Documents related to the leave request</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A new instance of <see cref="LeaveRequest"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<LeaveRequest> CreateAsync(ILeaveRequestRuleSet leaveRequestRuleSet,
                                                       EmployeeWithLeaveRequestsAndAllocation requestingEmployee,
                                                       LeaveType leaveType,
                                                       DateOnly startedAt,
                                                       DateOnly endedAt,
                                                       Employee approver,
                                                       Employee substitutor,
                                                       string? comment = null,
                                                       string? reasonDescription = null,
                                                       IEnumerable<EmployeeDocument>? employeeDocuments = null,
                                                       CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(
            requestingEmployee,
            leaveType,
            startedAt,
            endedAt,
            approver,
            leaveRequestRuleSet,
            cancellationToken
        );

        LeaveRequest leaveRequest = new()
        {
            RequestingEmployee = requestingEmployee.Employee,
            LeaveType = leaveType,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = endedAt.DayNumber - startedAt.DayNumber,
            Substitutor = substitutor,
            Comment = comment,
            Approver = approver,
            ReasonDescription = reasonDescription,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        if (employeeDocuments is not null)
        {
            leaveRequest._employeeDocuments.AddRange(employeeDocuments);
        }

        leaveRequest.AddEvent(new LeaveRequestCreated(leaveRequest));
        return leaveRequest;
    }

    /// <summary>
    /// Approves status of given leave request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting approval</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Approve(Employee requester)
    {
        if (requester != Approver)
        {
            throw new ArgumentException("Only approver is allowed to approve the request");
        }

        if (Status is not RequestStatus.Pending)
        {
            throw new InvalidOperationException("It is not allowed to approve canceled or rejected request");
        }

        UpdateStatus(RequestStatus.Approved);
    }

    /// <summary>
    /// Rejects status of given leave request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting rejection</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Reject(Employee requester)
    
    {
        if (requester != Approver)
        {
            throw new ArgumentException("Only approver is allowed to reject the request");
        }

        if (Status is not RequestStatus.Pending)
        {
            throw new InvalidOperationException("It is not allowed to reject canceled or approved request");
        }
        
        UpdateStatus(RequestStatus.Rejected);
    }

    /// <summary>
    /// Cancels status of given leave request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting cancelation</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Cancel(Employee requester)
    {
        if (requester != RequestingEmployee || requester != Approver)
        {
            throw new ArgumentException("Only approver or requesting-employee are allowed to cancel the request");
        }

        if (requester == RequestingEmployee && Status is not RequestStatus.Pending)
        {
            throw new InvalidOperationException("Only approver is allowed to cancel request after rejecting or approving");
        }

        UpdateStatus(RequestStatus.Canceled);
    }

    public void ChangeApproverOn(EmployeeWithLeaveRequests requestingApprover,
                                 DateOnly substitutionStartedAt,
                                 DateOnly substitutionEndedAt)
    {
        // check if new approver has no leave in given date
        var existingRequests = requestingApprover.LeaveRequests
            .Where(req => req.Status is RequestStatus.Approved or RequestStatus.Pending);

        foreach (var request in existingRequests)
        {
            var isOverlapsStart = request.EndedAt >= substitutionStartedAt && request.StartedAt <= substitutionStartedAt;
            var isOverlapsEnd = request.EndedAt >= substitutionEndedAt && request.StartedAt <= substitutionEndedAt;
            var isFullyContained = request.EndedAt <= substitutionEndedAt && request.StartedAt >= substitutionStartedAt;

            if (isOverlapsStart || isOverlapsEnd || isFullyContained)
            {
                throw new InvalidOperationException($"Given approver is on leave " +
                    $"during the period: {substitutionStartedAt:D}-{substitutionEndedAt:D}");
            }
        }

        Approver = requestingApprover.Employee;
    }

    private void UpdateStatus(RequestStatus status)
    {
        Status = status;
        DecidedAt = DateTime.UtcNow;
    }

    private static async Task ValidateBaseRulesAsync(EmployeeWithLeaveRequestsAndAllocation requestingEmployee,
                                                     LeaveType leaveType,
                                                     DateOnly startedAt,
                                                     DateOnly endedAt,
                                                     Employee approver,
                                                     ILeaveRequestRuleSet leaveRequestRuleSet,
                                                     CancellationToken cancellationToken)
    {
        if (requestingEmployee.Employee == approver)
        {
            throw new InvalidOperationException("Requesting employee cannot be their own approver");
        }

        var isRequestApproverSuperiorOfEmployee = await leaveRequestRuleSet
            .IsRequestApproverSuperiorOfEmployeeAsync(approver, requestingEmployee.Employee, cancellationToken);

        if (isRequestApproverSuperiorOfEmployee)
        {
            throw new InvalidOperationException("Approver must be a superior of requesting employee");
        }

        // think 'bout it
        //if (startedAt < DateOnly.FromDateTime(DateTime.UtcNow))
        //{
        //    throw new InvalidOperationException("Started date must be at least todays's date");
        //}

        if (startedAt > endedAt)
        {
            throw new InvalidOperationException("Request started date must be fewer than ended date");
        }

        int requestingDays = endedAt.DayNumber - startedAt.DayNumber; 
        int? remainingDaysInAllocation = requestingEmployee.LeaveAllocation.RemainingDays;
        var isLeaveLimited = remainingDaysInAllocation is not null;

        if (isLeaveLimited && requestingDays > remainingDaysInAllocation)
        {
            throw new InvalidOperationException("Request total days number must be fewer than available");
        }

        var existingRequests = requestingEmployee.LeaveRequests
            .Where(req => 
                req.LeaveType == leaveType && 
                req.Status is RequestStatus.Approved or RequestStatus.Pending
            );

        foreach (var request in existingRequests)
        {
            var isOverlapsStart = request.EndedAt >= startedAt && request.StartedAt <= startedAt;
            var isOverlapsEnd = request.EndedAt >= endedAt && request.StartedAt <= endedAt;
            var isFullyContained = request.EndedAt <= endedAt && request.StartedAt >= startedAt;

            if (isOverlapsStart || isOverlapsEnd || isFullyContained)
            {
                throw new InvalidOperationException("Given period for request is already included in another one");
            }
        }
    }

    #endregion
}
