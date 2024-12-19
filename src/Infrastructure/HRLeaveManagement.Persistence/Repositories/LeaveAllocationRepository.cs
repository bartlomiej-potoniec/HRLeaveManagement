using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveAllocationRepository(ApplicationDbContext dbContext) : ILeaveAllocationRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<LeaveAllocation>> GetAllAsync()
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync();

    public async Task<IEnumerable<LeaveAllocation>> GetAllByEmployeeIdAsync(Guid employeeId)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .Where(la => la.EmployeeId == employeeId)
            .ToListAsync();

    public async Task<LeaveAllocation?> GetByIdAsync(int id)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id);

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

    public async Task<bool> IsAllocationForEmployeeExistAsync(Guid employeeId, int leaveTypeId, int year)
        => await _dbContext.LeaveAllocations
            .AnyAsync(la => 
                la.EmployeeId == employeeId &&
                la.LeaveTypeId == leaveTypeId &&
                la.Year == year
            );

    public async Task CreateRangeAsync(IEnumerable<LeaveAllocation> leaveAllocations)
    {
        await _dbContext.AddRangeAsync(leaveAllocations);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(LeaveAllocation leaveAllocation)
    {
        _dbContext.LeaveAllocations.Update(leaveAllocation);
        await _dbContext.SaveChangesAsync();
    }
}
