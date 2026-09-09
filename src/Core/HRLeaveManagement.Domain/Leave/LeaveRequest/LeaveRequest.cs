using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee.ExternalContracts;
using HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

namespace HRLeaveManagement.Domain.Leave.LeaveRequest;

public class LeaveRequest : IRootEntity
{
    private readonly List<EmployeeDocument> _employeeDocuments = [];

    public int Id { get; private set; }
    public Guid RequestingEmployeeId { get; private set; }
    public int LeaveTypeId { get; private set; }
    public LeaveType.LeaveType LeaveType { get; private set; }
    public DateOnly StartedAt { get; private set; }
    public DateOnly EndedAt { get; private set; }
    public int TotalDays { get; private set; }
    public Guid SubstitutorId { get; private set; }
    public string? Comment { get; private set; }
    public Guid ApproverId { get; private set; }
    public string? ApproverComment { get; private set; }
    public string? ReasonDescription { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DecidedAt { get; private set; }
    public IReadOnlyList<EmployeeDocument> EmployeeDocuments => _employeeDocuments.AsReadOnly();

    private LeaveRequest() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="LeaveRequest"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="leaveRequestApproverSuperiorOfEmployeeChecker">Instance of <see cref="ILeaveRequestApproverSuperiorOfEmployeeChecker"/> for rules checking</param>
    /// <param name="requestingEmployeeId">Employee requesting leave</param>
    /// <param name="leaveType">Type of leave choosing by employee</param>
    /// <param name="startedAt">Date of leave starting</param>
    /// <param name="endedAt">Date of leave ending</param>
    /// <param name="approverId">Superior approving request</param>
    /// <param name="substitutorId">Employee substituting absent Employee</param>
    /// <param name="comment">Optional. Comment of requesting employee for leave request</param>
    /// <param name="reasonDescription">Optional. Description of reason of taking leave</param>
    /// <param name="employeeDocuments">Optional. Documents related to the leave request</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A new instance of <see cref="LeaveRequest"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<LeaveRequest> CreateAsync(ILeavePolicy leavePolicy,
                                                       ILeaveRequestApproverSuperiorOfEmployeeChecker leaveRequestApproverSuperiorOfEmployeeChecker,
                                                       ILeaveRequesterHasEnoughRemainingDaysChecker leaveRequesterHasEnoughRemainingDaysChecker,
                                                       IGivenApproverIsCurrentlyPresentChecker givenApproverIsCurrentlyPresentChecker,
                                                       INoAnotherLeaveRequestExistsInPeriodChecker noAnotherLeaveRequestExistsInPeriodChecker,
                                                       EmployeeLeaveInformation requestingEmployee,
                                                       Guid requestingEmployeeId,
                                                       LeaveType.LeaveType leaveType,
                                                       DateOnly startedAt,
                                                       DateOnly endedAt,
                                                       Guid approverId,
                                                       Guid substitutorId,
                                                       string? comment,
                                                       string? reasonDescription,
                                                       int currentYear,
                                                       IEnumerable<EmployeeDocument>? employeeDocuments,
                                                       CancellationToken cancellationToken = default)
    {
        // Ogólnie: Wszystkie abstrakcje przekazywane jako argument są zdefiniowane w module Leave

        // Zmodyfikowana przed chwilą polityka, dostarcza kontrakt Employee.EmployeeLeaveInformation 
        var isRequesterAllowedToUseLeaveType = leavePolicy.IsEligibleFor(requestingEmployee);
        if (!isRequesterAllowedToUseLeaveType)
        {
            throw new InvalidOperationException($"Requesting employee is not allowed to use leave type: {leaveType.Name}");
        }

        if (requestingEmployeeId == approverId)
        {
            throw new InvalidOperationException("Requesting employee cannot be their own approver");
        }
        
        // Klasa implementująca tę abstrakcję używa DbSetu Employee.
        var isRequestApproverSuperiorOfEmployee = await leaveRequestApproverSuperiorOfEmployeeChecker
            .IsEligible(approverId, requestingEmployeeId, cancellationToken);
        if (!isRequestApproverSuperiorOfEmployee)
        {
            throw new InvalidOperationException("Approver must be a superior of requesting employee");
        }

        // Tutaj klasa implementująca abstrakcje używa wyłącznie DbSetów z modułu Leave, więc jest ok
        int requestingDays = endedAt.DayNumber - startedAt.DayNumber;
        var hasRequesterEnoughRemainingDays = await leaveRequesterHasEnoughRemainingDaysChecker
            .IsEligible(requestingEmployeeId, requestingDays, currentYear, cancellationToken);
        if (!hasRequesterEnoughRemainingDays)
        {
            throw new InvalidOperationException("Request total days number must be fewer than available");
        }

        // Tutaj klasa implementująca abstrakcje używa wyłącznie DbSetów z modułu Leave, więc jest ok
        var hasRequesterNoAnotherLeaveInPeriod = await noAnotherLeaveRequestExistsInPeriodChecker
            .IsEligible(requestingEmployeeId, startedAt, endedAt);
        if (!hasRequesterNoAnotherLeaveInPeriod)
        {
            throw new InvalidOperationException("Given period for request is already included in another one");
        }

        // Tutaj klasa implementująca abstrakcje używa wyłącznie DbSetów z modułu Leave, więc jest ok
        var isApproverPresent = givenApproverIsCurrentlyPresentChecker
            .IsEligible(approverId, cancellationToken);
        if (!isApproverPresent)
        {
            throw new InvalidOperationException("Given approver is already not at the company");
        }

        LeaveRequest leaveRequest = new()
        {
            RequestingEmployeeId = requestingEmployeeId,
            LeaveType = leaveType,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = endedAt.DayNumber - startedAt.DayNumber,
            SubstitutorId = substitutorId,
            Comment = comment,
            ApproverId = approverId,
            ReasonDescription = reasonDescription,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        if (employeeDocuments is not null)
        {
            leaveRequest._employeeDocuments.AddRange(employeeDocuments);
        }

        return leaveRequest;
    }

    /// <summary>
    /// Approves status of given leave request
    /// </summary>
    /// <param name="requesterId">Given <see cref="Employee.Employee"/> requesting approval</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Approve(Guid requesterId)
    {
        if (requesterId != ApproverId)
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
    /// <param name="requesterId">Given <see cref="Employee"/> requesting rejection</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Reject(Guid requesterId)
    {
        if (requesterId != ApproverId)
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
    /// <param name="requesterId">Given <see cref="Employee"/> requesting cancelation</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Cancel(Guid requesterId)
    {
        if (requesterId != RequestingEmployeeId || requesterId != ApproverId)
        {
            throw new ArgumentException("Only approver or requesting-employee are allowed to cancel the request");
        }

        if (requesterId == RequestingEmployeeId && (Status is not RequestStatus.Pending or RequestStatus.Canceled))
        {
            throw new InvalidOperationException("Only approver is allowed to cancel request after rejecting or approving");
        }

        UpdateStatus(RequestStatus.Canceled);
    }

    public async Task ChangeApproverOnAsync(Guid requestingApproverId,
                                            DateOnly substitutionStartedAt,
                                            DateOnly substitutionEndedAt,
                                            INoAnotherLeaveRequestExistsInPeriodChecker noAnotherLeaveRequestExistsInPeriodChecker)
    {
        var hasRequesterNoAnotherLeaveInPeriod = await noAnotherLeaveRequestExistsInPeriodChecker
            .IsEligible(requestingApproverId, substitutionStartedAt, substitutionEndedAt);
        if (!hasRequesterNoAnotherLeaveInPeriod)
        {
            throw new InvalidOperationException("Given period for request is already included in another one");
        }

        ApproverId = requestingApproverId;
    }

    private void UpdateStatus(RequestStatus status)
    {
        Status = status;
        DecidedAt = DateTime.UtcNow;
    }

    #endregion
}
