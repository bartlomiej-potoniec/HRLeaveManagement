using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain;

public abstract class WorkRequest
{
    public int Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public int TotalDays { get; set; }

    public Guid ApproverId { get; set; }
    public RequestStatus Status { get; set; }
    public string? ApproverComment { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime DecidedAt { get; set; }
}