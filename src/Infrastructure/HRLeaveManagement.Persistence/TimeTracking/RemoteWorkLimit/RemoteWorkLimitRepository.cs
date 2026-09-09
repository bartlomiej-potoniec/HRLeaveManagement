using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;

namespace HRLeaveManagement.Persistence.TimeTracking.RemoteWorkLimit;

public sealed class RemoteWorkLimitRepository(ApplicationDbContext dbContext) : IRemoteWorkLimitRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<RemoteWorkLimit>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.RemoteWorkLimits.ToListAsync(cancellationToken);

    public async Task<IEnumerable<RemoteWorkLimit>> GetAllRemoteWorkLimitsByEmployeeIdAsync(Guid employeeId,
                                                                                            CancellationToken cancellationToken = default)
        => await _dbContext.RemoteWorkLimits
            .Where(rwl => rwl.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<RemoteWorkLimit?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.RemoteWorkLimits
            .FirstOrDefaultAsync(rml => rml.Id == id, cancellationToken);

    public async Task CreateAsync(RemoteWorkLimit remoteWorkLimit, CancellationToken cancellationToken = default)
    {
        await _dbContext.RemoteWorkLimits.AddAsync(remoteWorkLimit, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RemoteWorkLimit remoteWorkLimit, CancellationToken cancellationToken = default)
    {
        _dbContext.RemoteWorkLimits.Update(remoteWorkLimit);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);
}
