using HRLeaveManagement.Domain.Leave.LeaveRequest;

namespace HRLeaveManagement.Domain.WorkRequest;

public abstract class WorkRequest : IRootEntity
{
    public int Id { get; protected set; }

    public Guid RequestingEmployeeId { get; protected set; }
    public Employee.Employee RequestingEmployee { get; protected set; }

    public DateOnly StartedAt { get; protected set; }
    public DateOnly EndedAt { get; protected set; }
    public int TotalDays { get; protected set; }

    public Guid ApproverId { get; protected set; }
    public Employee.Employee Approver { get; protected set; }

    public string? ApproverComment { get; protected set; }
    public RequestStatus Status { get; protected set; }

    public DateTime CreatedAt { get; protected set; }
    public DateTime? DecidedAt { get; protected set; }

    protected WorkRequest() {}

    protected async Task InitializeBase<TRequest>(IWorkRequestRuleSet workRequestRuleSet,
                                                  Employee.Employee requestingEmployee,
                                                  DateOnly startedAt,
                                                  DateOnly endedAt,
                                                  Employee.Employee approver,
                                                  string? approverComment,
                                                  CancellationToken cancellationToken = default) 
        where TRequest : WorkRequest
    {
        await ValidateBaseRules<TRequest>(
            requestingEmployee,
            startedAt,
            endedAt,
            approver,
            workRequestRuleSet,
            cancellationToken
        );

        RequestingEmployee = requestingEmployee;
        StartedAt = startedAt;
        EndedAt = endedAt;
        TotalDays = endedAt.DayNumber - startedAt.DayNumber;
        Approver = approver;
        ApproverComment = approverComment;
        Status = RequestStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Approves status of given work request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting approval</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Approve(Employee.Employee requester)
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
    /// Rejects status of given work request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting rejection</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Reject(Employee.Employee requester)
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
    /// Cancels status of given work request
    /// </summary>
    /// <param name="requester">Given <see cref="Employee"/> requesting cancelation</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Cancel(Employee.Employee requester)
    {
        if (requester != RequestingEmployee || requester != Approver)
        {
            throw new ArgumentException("Only approver or requesting-employee are allowed to cancel the request");
        }

        if (requester != Approver && Status is not RequestStatus.Pending)
        {
            throw new InvalidOperationException("Only approver is allowed to cancel request after rejecting or approving");
        }

        UpdateStatus(RequestStatus.Canceled);
    }

    private void UpdateStatus(RequestStatus status)
    {
        Status = status;
        DecidedAt = DateTime.UtcNow;
    }

    private static async Task ValidateBaseRules<TRequest>(Employee.Employee requestingEmployee,
                                                          DateOnly startedAt,
                                                          DateOnly endedAt,
                                                          Employee.Employee approver,
                                                          IWorkRequestRuleSet workRequestRuleSet,
                                                          CancellationToken cancellationToken = default)
        where TRequest : WorkRequest
    {
        if (requestingEmployee == approver)
        {
            throw new ArgumentException("Requesting employee cannot be their own approver");
        }

        var isRequestApproverSuperiorOfEmployee = await workRequestRuleSet
            .IsRequestApproverSuperiorOfEmployeeAsync(approver, requestingEmployee, cancellationToken);

        if (isRequestApproverSuperiorOfEmployee)
        {
            throw new InvalidOperationException("Approver must be a superior of requesting employee");
        }

        if (startedAt > endedAt)
        {
            throw new InvalidOperationException("Request started date must be fewer than ended date");
        }

        var existingRequests = requestingEmployee.WorkRequests
            .Where(req => req is TRequest && req.Status is RequestStatus.Approved or RequestStatus.Pending);

        foreach (var request in existingRequests)
        {
            if (request.EndedAt >= endedAt && request.StartedAt <= endedAt ||
                request.EndedAt >= startedAt && request.StartedAt <= startedAt ||
                request.EndedAt <= endedAt && request.StartedAt >= startedAt)
            {
                throw new InvalidOperationException("Given period for request is already included in another one");
            }
        }
    }

    #endregion
}