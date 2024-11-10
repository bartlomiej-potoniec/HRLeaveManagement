using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveAllocationRepository(ApplicationDbContext dbContext) : ILeaveAllocationRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<LeaveAllocation?> GetUserLeaveAllocationsByIdAsync(string userId, int leaveTypeId)
        => await _dbContext.LeaveAllocations
            .FirstOrDefaultAsync(la => la.EmployeeId == Guid.Parse(userId) && la.LeaveTypeId == leaveTypeId);

    public async Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id);

    public async Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId)
        => await _dbContext.LeaveAllocations
            .Where(la => la.EmployeeId == Guid.Parse(userId))
            .Include(la => la.LeaveType)
            .ToListAsync();

    public async Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync()
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync();

    public async Task<bool> IsAllocationForUserExistAsync(Guid userId, int leaveTypeId, int year)
        => await _dbContext.LeaveAllocations
            .AnyAsync(la => 
                la.EmployeeId == userId &&
                la.LeaveTypeId == leaveTypeId &&
                la.Year == year
            );

    public async Task AddAllocationsAsync(IEnumerable<LeaveAllocation> leaveAllocations)
    {
        await _dbContext.AddRangeAsync(leaveAllocations);
        await _dbContext.SaveChangesAsync();
    }

    public Task<IEnumerable<LeaveAllocation>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LeaveAllocation?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LeaveAllocation leaveAllocation)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(LeaveAllocation leaveAllocation)
    {
        throw new NotImplementedException();
    }
}
