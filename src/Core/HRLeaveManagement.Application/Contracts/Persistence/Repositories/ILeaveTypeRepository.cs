using HRLeaveManagement.Domain.Leave.LeaveType;

namespace HRLeaveManagement.Application.Contracts.Persistence.Repositories;

public interface ILeaveTypeRepository
{
    Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LeaveType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(LeaveType leaveType, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaveType leaveType, CancellationToken cancellationToken = default);
    Task DeleteAsync(LeaveType leaveType, CancellationToken cancellationToken = default);

    Task<bool> IsLeaveTypeUniqueAsync(string name, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
