using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Domain.Leave.LeaveAllocation;

namespace HRLeaveManagement.Persistence.Leave.LeaveAllocation;

public sealed class LeaveAllocationRepository(ApplicationDbContext dbContext) : ILeaveAllocationRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<LeaveAllocation>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<LeaveAllocation>> GetAllByEmployeeIdAsync(Guid employeeId,
                                                                            CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .Where(la => la.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<LeaveAllocation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id, cancellationToken);

    public async Task<LeaveAllocation?> GetUserLeaveAllocationByIdAsync(Guid employeeId,
                                                                        int leaveTypeId,
                                                                        int year,
                                                                        CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .FirstOrDefaultAsync(la => 
                la.EmployeeId == employeeId && 
                    la.LeaveTypeId == leaveTypeId &&
                    la.Year == year,
                cancellationToken
            );

    public async Task<LeaveAllocation?> GetLeaveAllocationWithDetailsByIdAsync(int id,
                                                                               CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .FirstOrDefaultAsync(la => la.Id == id, cancellationToken);

    public async Task<IReadOnlyList<LeaveAllocation>> GetUserLeaveAllocationsWithDetailsAsync(string userId,
                                                                                              CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Where(la => la.EmployeeId == Guid.Parse(userId))
            .Include(la => la.LeaveType)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<LeaveAllocation>> GetAllLeaveAllocationsWithDetailsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .Include(la => la.LeaveType)
            .ToListAsync(cancellationToken);

    public async Task<bool> IsAllocationForEmployeeExistAsync(Guid employeeId,
                                                              int leaveTypeId,
                                                              int year,
                                                              CancellationToken cancellationToken = default)
        => await _dbContext.LeaveAllocations
            .AnyAsync(la => 
                la.EmployeeId == employeeId &&
                    la.LeaveTypeId == leaveTypeId &&
                    la.Year == year,
                cancellationToken
            );

    public async Task CreateRangeAsync(IEnumerable<LeaveAllocation> leaveAllocations, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddRangeAsync(leaveAllocations, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(LeaveAllocation leaveAllocation, CancellationToken cancellationToken = default)
    {
        _dbContext.LeaveAllocations.Update(leaveAllocation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
