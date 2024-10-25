using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class ExtraRemoteWorkRequest : WorkRequest
{
    public string? ReasonDescription { get; private set; }

    private ExtraRemoteWorkRequest() { }


    // Factory Methods
    public static ExtraRemoteWorkRequest Create(Guid employeeId,
                                                DateTime startedAt,
                                                DateTime endedAt,
                                                Guid approverId,
                                                string? approverComment,
                                                string? reasonDescription)
    => new()
    {
        EmployeeId = employeeId,
        StartedAt = startedAt,
        EndedAt = endedAt,
        TotalDays = (int)(endedAt - startedAt).TotalDays,
        ApproverId = approverId,
        ApproverComment = approverComment,
        Status = RequestStatus.Pending,
        ReasonDescription = reasonDescription,
        CreatedAt = DateTime.UtcNow
    };
}