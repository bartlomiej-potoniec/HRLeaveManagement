using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveTypeRepository(ApplicationDbContext dbContext) : ILeaveTypeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeaveTypes.ToListAsync(cancellationToken);

    public async Task<LeaveType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == id, cancellationToken);

    public async Task<bool> IsLeaveTypeUniqueAsync(string name, CancellationToken cancellationToken = default)
        => !(await _dbContext.LeaveTypes.AnyAsync(lt => lt.Name == name, cancellationToken));

    public async Task CreateAsync(LeaveType leaveType, CancellationToken cancellationToken = default)
    {
        await _dbContext.LeaveTypes.AddAsync(leaveType, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(LeaveType leaveType, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(leaveType);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(LeaveType leaveType, CancellationToken cancellationToken = default)
    {
        _dbContext.LeaveTypes.Remove(leaveType);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}
