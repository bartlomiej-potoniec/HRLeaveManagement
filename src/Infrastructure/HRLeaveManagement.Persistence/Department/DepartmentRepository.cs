using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Domain.Department;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Department;

public sealed class DepartmentRepository(ApplicationDbContext dbContext) : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Departments.ToListAsync(cancellationToken);

    public async Task<Department?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default)
        => await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

    public async Task<Department?> GetWithDetailsById(int departmentId, CancellationToken cancellationToken = default)
        => await _dbContext.Departments
            .Include(d => d.Sections)
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

    public async Task CreateAsync(Department department, CancellationToken cancellationToken = default)
    {
        await _dbContext.Departments.AddAsync(department, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) 
        => await _dbContext.SaveChangesAsync(cancellationToken);

}
