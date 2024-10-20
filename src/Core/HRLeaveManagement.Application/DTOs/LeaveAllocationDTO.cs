namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveAllocationDTO
{
    public required int Id { get; init; }
    public required int NumberOfDays { get; init; }
    public required LeaveTypeDTO LeaveType { get; init; }
    public required int LeaveTypeId { get; init; }
    public required int Period { get; init; }
}
