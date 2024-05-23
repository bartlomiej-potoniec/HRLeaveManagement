namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveRequestDetailsDTO
{
    public required string RequestedEmployeeId { get; init; }

    public required LeaveTypeDTO LeaveType { get; init; }
    public required int LeaveTypeId { get; init; }
    
    public required DateTime RequestedAt { get; init; }
    public required DateTime StartedAt { get; init; }
    public required DateTime EndedAt { get; init; }

    public required string RequestComment { get; init; }
    
    public DateTime? ActionedAt { get; init; }

    public bool? IsApproved { get; set; }
    public required bool IsCancelled { get; set; }
}
