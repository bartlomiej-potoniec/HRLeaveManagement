using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveRequestRepository(ApplicationDbContext dbContext) : ILeaveRequestRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id)
        => await _dbContext.LeaveRequests
            .Include(lr => lr.LeaveType)
            .FirstOrDefaultAsync(lr => lr.Id == id);
    
    public async Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync()
        => await _dbContext.LeaveRequests
            .Include(lr => lr.LeaveType)
            .ToListAsync();

    public async Task<IReadOnlyList<LeaveRequest>> GetEmployeeLeaveRequestsWithDetailsAsync(Guid userId)
        => await _dbContext.LeaveRequests
            .Where(lr => lr.RequestingEmployeeId == userId)
            .Include(lr => lr.LeaveType)
            .ToListAsync();

    public Task<LeaveRequest?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LeaveRequest leaveRequest)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(LeaveRequest leaveRequest)
    {
        throw new NotImplementedException();
    }
}
