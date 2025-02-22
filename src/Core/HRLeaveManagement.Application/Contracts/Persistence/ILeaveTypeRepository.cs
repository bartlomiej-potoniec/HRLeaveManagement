using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface ILeaveTypeRepository
{
    Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LeaveType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(LeaveType leaveType, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveType leaveType, CancellationToken cancellationToken = default);
    Task DeleteAsync(LeaveType leaveType, CancellationToken cancellationToken = default);

    Task<bool> IsLeaveTypeUniqueAsync(string name, CancellationToken cancellationToken = default);
}
