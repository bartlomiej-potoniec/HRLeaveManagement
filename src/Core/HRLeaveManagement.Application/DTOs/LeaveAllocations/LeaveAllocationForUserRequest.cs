namespace HRLeaveManagement.Application.DTOs.LeaveAllocations;

public record LeaveAllocationForUserRequest(int LeaveTypeId, int? AvailableDays);
