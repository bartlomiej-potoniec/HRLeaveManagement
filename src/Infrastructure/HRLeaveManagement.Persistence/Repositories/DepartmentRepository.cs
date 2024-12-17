using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class DepartmentRepository(ApplicationDbContext dbContext) : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Department>> GetAllAsync()
        => await _dbContext.Departments.ToListAsync();

    public async Task<Department?> GetByIdAsync(int departmentId)
        => await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId);

    public async Task CreateAsync(Department department)
    {
        await _dbContext.Departments.AddAsync(department);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();
    }
}
