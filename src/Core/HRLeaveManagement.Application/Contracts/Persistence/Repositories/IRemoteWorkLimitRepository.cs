using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence.Repositories;

public interface IRemoteWorkLimitRepository
{
    Task<IEnumerable<RemoteWorkLimit>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RemoteWorkLimit>> GetAllRemoteWorkLimitsByEmployeeIdAsync(Guid employeeId,
                                                                               CancellationToken cancellationToken = default);
    Task<RemoteWorkLimit?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task CreateAsync(RemoteWorkLimit remoteWorkLimit, CancellationToken cancellationToken = default);
    Task UpdateAsync(RemoteWorkLimit remoteWorkLimit, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
