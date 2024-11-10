using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveTypeRepository(ApplicationDbContext dbContext) : ILeaveTypeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<int> CreateAsync(LeaveType leaveType)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(LeaveType leaveType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LeaveType>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LeaveType> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsLeaveTypeUnique(string name)
        => !await _dbContext.LeaveTypes.AnyAsync(lt => lt.Name == name);

    public Task UpdateAsync(LeaveType leaveType)
    {
        throw new NotImplementedException();
    }
}
