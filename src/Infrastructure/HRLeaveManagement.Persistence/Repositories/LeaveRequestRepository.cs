using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveRequestRepository(ApplicationDbContext context)
    : GenericRepository<LeaveRequest>(context), ILeaveRequestRepository
{
    public async Task<LeaveRequest?> GetLeaveRequestWithDetailsByIdAsync(int id)
        => await _context.LeaveRequests
            .Include(lr => lr.LeaveType)
            .FirstOrDefaultAsync(lr => lr.Id == id);
    
    public async Task<IReadOnlyList<LeaveRequest>> GetAllLeaveRequestsWithDetailsAsync()
        => await _context.LeaveRequests
            .Include(lr => lr.LeaveType)
            .ToListAsync();

    public async Task<IReadOnlyList<LeaveRequest>> GetUserLeaveRequestsWithDetailsAsync(string userId)
        => await _context.LeaveRequests
            .Where(lr => lr.RequestingEmployeeId == userId)
            .Include(lr => lr.LeaveType)
            .ToListAsync();
}
