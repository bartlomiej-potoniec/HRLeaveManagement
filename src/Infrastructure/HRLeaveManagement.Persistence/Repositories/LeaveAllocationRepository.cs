using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveAllocationRepository(ApplicationDbContext context)
    : GenericRepository<LeaveAllocation>(context), ILeaveAllocationRepository
{
    public async Task<LeaveAllocation?> GetUserLeaveAllocationsByIdAsync(string userId,
                                                                        int leaveTypeId)
        => await _context.LeaveAllocations
            .FirstOrDefaultAsync(la => la.EmployeeId == userId && la.LeaveTypeId == leaveTypeId);

    public async Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id)
        => await _context.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id);

    public async Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId)
        => await _context.LeaveAllocations
            .Where(la => la.EmployeeId == userId)
            .Include(la => la.LeaveType)
            .ToListAsync();

    public async Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync()
        => await _context.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync();

    public async Task<bool> IsAllocationExistAsync(string userId, int leaveTypeId, int period)
        => await _context.LeaveAllocations
            .AnyAsync(la => la.EmployeeId == userId
                && la.LeaveTypeId == leaveTypeId
                && la.Period == period
            );

    public async Task AddAllocationsAsync(IEnumerable<LeaveAllocation> leaveAllocations)
    {
        await _context.AddRangeAsync(leaveAllocations);
        await _context.SaveChangesAsync();
    }
}
