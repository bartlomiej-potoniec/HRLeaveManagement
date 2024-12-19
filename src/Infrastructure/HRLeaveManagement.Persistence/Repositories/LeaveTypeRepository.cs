using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveTypeRepository(ApplicationDbContext dbContext) : ILeaveTypeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<LeaveType>> GetAllAsync()
        => await _dbContext.LeaveTypes.ToListAsync();

    public async Task<LeaveType?> GetByIdAsync(int id)
        => await _dbContext.LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == id);

    public async Task<bool> IsLeaveTypeUniqueAsync(string name)
        => !(await _dbContext.LeaveTypes.AnyAsync(lt => lt.Name == name));

    public async Task CreateAsync(LeaveType leaveType)
    {
        await _dbContext.LeaveTypes.AddAsync(leaveType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(LeaveType leaveType)
    {
        _dbContext.Update(leaveType);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(LeaveType leaveType)
    {
        _dbContext.LeaveTypes.Remove(leaveType);
        await _dbContext.SaveChangesAsync();
    }
}
