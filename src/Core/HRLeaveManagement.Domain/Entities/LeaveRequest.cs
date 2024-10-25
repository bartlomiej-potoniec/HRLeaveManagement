using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class LeaveRequest
{
    public int Id { get; private set; }

    public Guid RequestingEmployeeId { get; private set; }

    public int LeaveTypeId { get; private set; }
    public LeaveType? LeaveType { get; private set; }

    public DateTime StartedAt { get; private set; }
    public DateTime EndedAt { get; private set; }
    public int TotalDays { get; private set; }

    public Guid SubstitutorId { get; private set; }
    public string? Comment { get; private set; }

    public Guid ApproverId { get; private set; }
    public string? ApproverComment { get; private set; }

    public string? ReasonDescription { get; private set; }
    public string? DocumentPath { get; private set; }

    public RequestStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? DecidedAt { get; private set; }

    private LeaveRequest() { }


    // Factory Methods
    public static LeaveRequest Create(Guid requestingEmployeeId,
                                      int leaveTypeId,
                                      DateTime startedAt,
                                      DateTime endedAt,
                                      Guid substitutorId,
                                      string? comment,
                                      Guid approverId,
                                      string? approverComment,
                                      string? reasonDescription,
                                      string? documentPath)
        => new()
        {
            RequestingEmployeeId = requestingEmployeeId,
            LeaveTypeId = leaveTypeId,
            StartedAt = startedAt,
            EndedAt = endedAt,
            TotalDays = (int)(endedAt - startedAt).TotalDays,
            SubstitutorId = substitutorId,
            Comment = comment,
            ApproverId = approverId,
            ApproverComment = approverComment,
            ReasonDescription = reasonDescription,
            DocumentPath = documentPath,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

    public static void Approve(LeaveRequest entity)
        => UpdateStatus(entity, RequestStatus.Approved);

    public static void Reject(LeaveRequest entity)
        => UpdateStatus(entity, RequestStatus.Rejected);

    public static void Cancel(LeaveRequest entity)
        => UpdateStatus(entity, RequestStatus.Canceled);

    private static void UpdateStatus(LeaveRequest entity, RequestStatus status)
    {
        entity.Status = status;
        entity.DecidedAt = DateTime.UtcNow;
    }
}
