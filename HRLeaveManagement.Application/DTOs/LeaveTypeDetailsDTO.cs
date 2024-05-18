namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveTypeDetailsDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int DefaultDays { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
}
