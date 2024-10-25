using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class OvertimeRequest : WorkRequest
{
    public string? PurposeDescription { get; private set; }

    private OvertimeRequest() { }


    // Factory Methods
    public static OvertimeRequest Create(Guid employeeId,
                                         DateTime startedAt,
                                         DateTime endedAt,
                                         Guid approverId,
                                         string? approverComment,
                                         string? purposeDescription)
        => new()
        {
            EmployeeId = employeeId,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = (int)(endedAt - startedAt).TotalDays,
            ApproverId = approverId,
            ApproverComment = approverComment,
            Status = RequestStatus.Pending,
            PurposeDescription = purposeDescription,
            CreatedAt = DateTime.UtcNow
        };
}