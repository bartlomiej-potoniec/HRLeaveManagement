namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveTypeDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int DefaultDays { get; init; }
}