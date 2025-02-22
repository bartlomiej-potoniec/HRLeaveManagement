using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext dbContext) : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmploymentContracts)
            .ToListAsync(cancellationToken);

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmploymentContracts)
            .Include(e => e.EmployeeEducations)
            .Include(e => e.EmployeeExperiences)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task CreateWithDetailsAsync(Employee employee,
                                             EmployeeContract employeeContract,
                                             IEnumerable<EmployeeEducation> employeeEducations,
                                             IEnumerable<EmployeeExperience> employeeExperiences,
                                             CancellationToken cancellationToken = default)
    {
        await _dbContext.Employees.AddAsync(employee, cancellationToken);
        await _dbContext.EmployeeContracts.AddAsync(employeeContract, cancellationToken);
        await _dbContext.EmployeeEducations.AddRangeAsync(employeeEducations, cancellationToken);
        await _dbContext.EmployeeExperiences.AddRangeAsync(employeeExperiences, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateBasicInfoAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWithDetailsAsync(Employee employee,
                                             IEnumerable<EmployeeContract> contractsToCreate,
                                             IEnumerable<EmployeeEducation> educationsToCreate,
                                             IEnumerable<EmployeeExperience> experiencesToCreate,
                                             IEnumerable<EmployeeContract> contractsToUpdate,
                                             IEnumerable<EmployeeEducation> educationsToUpdate,
                                             IEnumerable<EmployeeExperience> experiencesToUpdate,
                                             IEnumerable<EmployeeContract> contractsToDelete,
                                             IEnumerable<EmployeeEducation> educationsToDelete,
                                             IEnumerable<EmployeeExperience> experiencesToDelete,
                                             CancellationToken cancellationToken = default)
    {
        _dbContext.Employees.Update(employee);

        _dbContext.EmployeeContracts.UpdateRange(contractsToUpdate);
        _dbContext.EmployeeEducations.UpdateRange(educationsToUpdate);
        _dbContext.EmployeeExperiences.UpdateRange(experiencesToUpdate);

        _dbContext.EmployeeContracts.RemoveRange(contractsToDelete);
        _dbContext.EmployeeEducations.RemoveRange(educationsToDelete);
        _dbContext.EmployeeExperiences.RemoveRange(experiencesToDelete);

        await _dbContext.EmployeeContracts.AddRangeAsync(contractsToCreate, cancellationToken);
        await _dbContext.EmployeeEducations.AddRangeAsync(educationsToCreate, cancellationToken);
        await _dbContext.EmployeeExperiences.AddRangeAsync(experiencesToCreate, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId,
                                                                                      CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeContracts
            .Where(ec => ec.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<EmployeeContract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeContracts
            .FirstOrDefaultAsync(ec => ec.Id == contractId, cancellationToken);

    public async Task CreateEmployeeContract(EmployeeContract employeeContract, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(employeeContract, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
