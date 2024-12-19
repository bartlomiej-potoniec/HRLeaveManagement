using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface IRemoteWorkLimitRepository
{
    Task<IEnumerable<RemoteWorkLimit>> GetAllAsync();
    Task<IEnumerable<RemoteWorkLimit>> GetAllRemoteWorkLimitsByEmployeeIdAsync(Guid employeeId);
    Task<RemoteWorkLimit?> GetByIdAsync(int id);

    Task Create(RemoteWorkLimit remoteWorkLimit);
    Task Update(RemoteWorkLimit remoteWorkLimit);
}
