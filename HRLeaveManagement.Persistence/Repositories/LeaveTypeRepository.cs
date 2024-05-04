using HRLeaveManagement.Domain;
using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class LeaveTypeRepository(ApplicationDbContext context)
    : GenericRepository<LeaveType>(context), ILeaveTypeRepository
{
    public async Task<bool> IsLeaveTypeUnique(string name)
        => await _context.LeaveTypes.AnyAsync(lt => lt.Name == name);
    
}
