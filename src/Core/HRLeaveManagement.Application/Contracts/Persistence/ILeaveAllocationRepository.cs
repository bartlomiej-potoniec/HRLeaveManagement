using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveAllocationRepository
{
    Task<IEnumerable<LeaveAllocation>> GetAllAsync();
    Task<LeaveAllocation?> GetByIdAsync(int id);
    Task UpdateAsync(LeaveAllocation leaveAllocation);
    Task DeleteAsync(LeaveAllocation leaveAllocation);
    Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id);
    Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync();
    Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId);
    Task<LeaveAllocation?> GetUserLeaveAllocationsByIdAsync(string userId, int leaveTypeId);
    Task AddAllocationsAsync(IEnumerable<LeaveAllocation> leaveAllocations);
    Task<bool> IsAllocationForUserExistAsync(Guid userId, int leaveTypeId, int year);
}