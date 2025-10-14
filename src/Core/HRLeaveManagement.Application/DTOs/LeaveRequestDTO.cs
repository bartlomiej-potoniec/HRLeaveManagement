using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.DTOs.LeaveTypes;
using HRLeaveManagement.Application.DTOs.Users;

namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveRequestDTO
{
    public required int Id { get; init; }
    public required EmployeeDTO Employee { get; init; }
    public required string RequestingEmployeeId { get; init; }
    public required LeaveTypeDTO LeaveType { get; init; }
    public required DateTime RequestedAt { get; init; }
    public required DateTime StartedAt { get; init; }
    public required DateTime EndedAt { get; init; }
    public bool? IsApproved { get; init; }
    public bool? IsCanceled { get; init; }
}
