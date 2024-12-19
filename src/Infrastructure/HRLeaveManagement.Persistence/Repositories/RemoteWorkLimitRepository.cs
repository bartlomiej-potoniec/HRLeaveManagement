using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class RemoteWorkLimitRepository(ApplicationDbContext dbContext) : IRemoteWorkLimitRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<RemoteWorkLimit>> GetAllAsync()
        => await _dbContext.RemoteWorkLimits.ToListAsync();

    public async Task<IEnumerable<RemoteWorkLimit>> GetAllRemoteWorkLimitsByEmployeeIdAsync(Guid employeeId)
        => await _dbContext.RemoteWorkLimits
            .Where(rwl => rwl.EmployeeId == employeeId)
            .ToListAsync();

    public async Task<RemoteWorkLimit?> GetByIdAsync(int id)
        => await _dbContext.RemoteWorkLimits.FirstOrDefaultAsync(rml => rml.Id == id);

    public async Task CreateAsync(RemoteWorkLimit remoteWorkLimit)
    {
        await _dbContext.RemoteWorkLimits.AddAsync(remoteWorkLimit);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RemoteWorkLimit remoteWorkLimit)
    {
        _dbContext.RemoteWorkLimits.Update(remoteWorkLimit);
        await _dbContext.SaveChangesAsync();
    }
}
