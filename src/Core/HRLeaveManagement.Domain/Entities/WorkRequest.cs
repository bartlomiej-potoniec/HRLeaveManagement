using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public abstract class WorkRequest
{
    public int Id { get; protected set; }
    public Guid EmployeeId { get; protected set; }
    public DateTime StartedAt { get; protected set; }
    public DateTime EndedAt { get; protected set; }
    public int TotalDays { get; protected set; }

    public Guid ApproverId { get; protected set; }
    public string? ApproverComment { get; protected set; }
    public RequestStatus Status { get; protected set; }

    public DateTime CreatedAt { get; protected set; }
    public DateTime? DecidedAt { get; protected set; }


    // Factory Methods
    public static void Approve(WorkRequest entity)
        => UpdateStatus(entity, RequestStatus.Approved);

    public static void Reject(WorkRequest entity)
        => UpdateStatus(entity, RequestStatus.Rejected);

    public static void Cancel(WorkRequest entity)
        => UpdateStatus(entity, RequestStatus.Canceled);

    private static void UpdateStatus(WorkRequest entity, RequestStatus status)
    {
        entity.Status = status;
        entity.DecidedAt = DateTime.UtcNow;
    }
}