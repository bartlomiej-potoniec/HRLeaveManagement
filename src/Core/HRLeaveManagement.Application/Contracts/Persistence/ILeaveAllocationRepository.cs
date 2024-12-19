using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveAllocationRepository
{
    Task<IEnumerable<LeaveAllocation>> GetAllAsync();
    Task<IEnumerable<LeaveAllocation>> GetAllByEmployeeIdAsync(Guid employeeId);
    Task<LeaveAllocation?> GetByIdAsync(int id);
    Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id);
    Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync();
    Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId);
    Task<LeaveAllocation?> GetUserLeaveAllocationsByIdAsync(string userId, int leaveTypeId);

    Task<bool> IsAllocationForEmployeeExistAsync(Guid employeeId, int leaveTypeId, int year);
    
    Task CreateRangeAsync(IEnumerable<LeaveAllocation> leaveAllocations);
    Task UpdateAsync(LeaveAllocation leaveAllocation);
}