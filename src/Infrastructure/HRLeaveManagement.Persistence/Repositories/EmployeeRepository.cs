using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext dbContext) : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmploymentContracts)
            .ToListAsync();

    public async Task<Employee?> GetByIdAsync(Guid id)
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmploymentContracts)
            .Include(e => e.EmployeeEducations)
            .Include(e => e.EmployeeExperiences)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task CreateWithDetailsAsync(Employee employee,
                                             EmployeeContract employeeContract,
                                             IEnumerable<EmployeeEducation> employeeEducations,
                                             IEnumerable<EmployeeExperience> employeeExperiences)
    {
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.EmployeeContracts.AddAsync(employeeContract);
        await _dbContext.EmployeeEducations.AddRangeAsync(employeeEducations);
        await _dbContext.EmployeeExperiences.AddRangeAsync(employeeExperiences);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBasicInfoAsync(Employee employee)
    {
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateWithDetailsAsync(Employee employee,
                                             IEnumerable<EmployeeContract> contractsToCreate,
                                             IEnumerable<EmployeeEducation> educationsToCreate,
                                             IEnumerable<EmployeeExperience> experiencesToCreate,
                                             IEnumerable<EmployeeContract> contractsToUpdate,
                                             IEnumerable<EmployeeEducation> educationsToUpdate,
                                             IEnumerable<EmployeeExperience> experiencesToUpdate)
    {
        _dbContext.Employees.Update(employee);

        _dbContext.EmployeeContracts.UpdateRange(contractsToUpdate);
        _dbContext.EmployeeEducations.UpdateRange(educationsToUpdate);
        _dbContext.EmployeeExperiences.UpdateRange(experiencesToUpdate);

        await _dbContext.EmployeeContracts.AddRangeAsync(contractsToCreate);
        await _dbContext.EmployeeEducations.AddRangeAsync(educationsToCreate);
        await _dbContext.EmployeeExperiences.AddRangeAsync(experiencesToCreate);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId)
        => await _dbContext.EmployeeContracts
            .Where(ec => ec.EmployeeId == employeeId)
            .ToListAsync();

    public async Task<EmployeeContract?> GetContractByIdAsync(int contractId)
        => await _dbContext.EmployeeContracts
            .FirstOrDefaultAsync(ec => ec.Id == contractId);

    public async Task CreateEmployeeContract(EmployeeContract employeeContract)
    {
        await _dbContext.AddAsync(employeeContract);
        await _dbContext.SaveChangesAsync();
    }
}
