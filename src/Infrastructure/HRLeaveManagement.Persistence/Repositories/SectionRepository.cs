using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class SectionRepository(ApplicationDbContext dbContext) : ISectionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Section?> GetByIdAsync(int id)
        => await _dbContext.Sections
            .Include(s => s.Department)
            .Include(s => s.Leader)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Section>> GetAllByDepartmentIdAsync(int departmentId)
        => await _dbContext.Sections
            .Include(s => s.Department)
            .Include(s => s.Leader)
            .Where(s => s.DepartmentId == departmentId)
            .ToListAsync();
}
