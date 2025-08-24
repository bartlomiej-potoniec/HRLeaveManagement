using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveRequestRepository(ApplicationDbContext dbContext) : ILeaveRequestRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    public Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id,
                                                                         CancellationToken cancellationToken = default)
        => await _dbContext.LeaveRequests
            .Include(lr => lr.LeaveType)
            .FirstOrDefaultAsync(lr => lr.Id == id, cancellationToken);
    
    public async Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeaveRequests
            .Include(lr => lr.LeaveType)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<LeaveRequest>> GetEmployeeLeaveRequestsWithDetailsAsync(Guid userId,
                                                                                            CancellationToken cancellationToken = default)
        => await _dbContext.LeaveRequests
            .Where(lr => lr.RequestingEmployeeId == userId)
            .Include(lr => lr.LeaveType)
            .ToListAsync(cancellationToken);
}
