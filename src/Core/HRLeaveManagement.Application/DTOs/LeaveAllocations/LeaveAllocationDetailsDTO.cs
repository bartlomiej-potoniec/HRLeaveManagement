using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.DTOs.LeaveTypes;

namespace HRLeaveManagement.Application.DTOs.LeaveAllocations;

public sealed record LeaveAllocationDetailsDTO
{
    public required int Id { get; init; }
    public required LeaveTypeDTO LeaveType { get; init; }
    public required Guid EmployeeId { get; init; }
    public int? AvailableDays { get; init; }
    public int? RemainingDays { get; init; }
    public int? UsedDays { get; init; }
    public required int Year { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }

}
