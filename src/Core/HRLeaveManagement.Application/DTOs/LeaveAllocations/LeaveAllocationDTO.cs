namespace HRLeaveManagement.Application.DTOs.LeaveAllocations;

public record LeaveAllocationDTO
{
    public required int Id { get; init; }
    public required int LeaveTypeId { get; init; }
    public required string LeaveTypeName { get; init; }
    public required Guid EmployeeId { get; init; }
    public required string EmployeeName { get; init; }
    public int? AvailableDays { get; init; }
    public int? RemainingDays { get; init; }
    public int? UsedDays { get; init; }
    public required int Year { get; init; }
}
