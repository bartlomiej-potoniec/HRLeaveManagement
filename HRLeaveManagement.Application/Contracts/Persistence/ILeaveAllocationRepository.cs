using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
{
    Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id);
    Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync();
    Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId);
    Task<LeaveAllocation?> GetUserLeaveAllocationsByIdAsync(string userId, int leaveTypeId);
    Task AddAllocationsAsync(IEnumerable<LeaveAllocation> leaveAllocations);
    Task<bool> IsAllocationExistAsync(string userId, int leaveTypeId, int period);
}