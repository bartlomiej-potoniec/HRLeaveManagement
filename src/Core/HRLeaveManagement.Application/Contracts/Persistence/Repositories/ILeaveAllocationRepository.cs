using HRLeaveManagement.Domain.Leave.LeaveAllocation;

namespace HRLeaveManagement.Application.Contracts.Persistence.Repositories;

public interface ILeaveAllocationRepository
{
    Task<IEnumerable<LeaveAllocation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LeaveAllocation>> GetAllByEmployeeIdAsync(Guid employeeId,
                                                               CancellationToken cancellationToken = default);
    Task<LeaveAllocation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id,
                                                                  CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId,
                                                                                 CancellationToken cancellationToken = default);
    Task<LeaveAllocation?> GetUserLeaveAllocationByIdAsync(Guid employeeId,
                                                           int leaveTypeId,
                                                           int year,
                                                           CancellationToken cancellationToken = default);

    Task<bool> IsAllocationForEmployeeExistAsync(Guid employeeId,
                                                 int leaveTypeId,
                                                 int year,
                                                 CancellationToken cancellationToken = default);

    Task CreateRangeAsync(IEnumerable<LeaveAllocation> leaveAllocations, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveAllocation leaveAllocation, CancellationToken cancellationToken = default);
}