using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class SectionRepository(ApplicationDbContext dbContext) : ISectionRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Section>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Sections
            .Include(s => s.Department)
            .ToListAsync(cancellationToken);

    public async Task<Section?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Sections
            .Include(s => s.Department)
            .Include(s => s.Leader)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IEnumerable<Section>> GetAllByDepartmentIdAsync(int departmentId,
                                                                      CancellationToken cancellationToken = default)
        => await _dbContext.Sections
            .Include(s => s.Department)
            .Include(s => s.Leader)
            .Where(s => s.DepartmentId == departmentId)
            .ToListAsync(cancellationToken);

    public async Task CreateAsync(Section section, CancellationToken cancellationToken = default)
    {
        await _dbContext.Sections.AddAsync(section, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Section section, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(section);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
