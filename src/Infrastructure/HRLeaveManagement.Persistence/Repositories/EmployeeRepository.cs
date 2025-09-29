using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
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
            .Include(e => e.EmployeeContracts)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Employee>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmployeeContracts)
            .Include(e => e.EmployeeEducations)
            .Include(e => e.EmployeeExperiences)
            .Include(e => e.LeaveRequests)
            .Include(e => e.LeaveAllocations)
            .Include(e => e.WorkRequests)
            .Include(e => e.DelegationRequests)
            .Include(e => e.ExtraRemoteWorkRequests)
            .Include(e => e.OvertimeRequests)
            .Include(e => e.RemoteWorkLimits)
            .Include(e => e.TimeRegisters)
            .ToListAsync(cancellationToken);

    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmployeeContracts)
            .Include(e => e.EmployeeEducations)
            .Include(e => e.EmployeeExperiences)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Employee?> GetWithContractsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.EmployeeContracts)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Employee?> GetWithRemoteWorkLimitsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.RemoteWorkLimits)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Employee?> GetWithLeaveAllocationsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.LeaveAllocations)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Employee?> GetWithLeaveRequestsAndAllocationByIdAsync(Guid id,
                                                                            int leaveTypeId,
                                                                            int currentYear,
                                                                            CancellationToken cancellationToken = default)
        => await _dbContext.Employees
            .Include(e => e.LeaveRequests)
            .Include(e => e.LeaveAllocations)
            .Where(
                e => e.Id == id && 
                e.LeaveAllocations.Any(la => la.LeaveTypeId == leaveTypeId && la.Year == currentYear)
            )
            .FirstOrDefaultAsync(cancellationToken);
            

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        => await _dbContext.Employees.AddAsync(employee, cancellationToken);

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

    public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _dbContext.Employees.Update(employee);
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
        await _dbContext.EmployeeContracts.AddAsync(employeeContract, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeEducation>> GetAllEducationsByEmployeeIdAsync(Guid employeeId,
                                                                                        CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeEducations
            .Where(ed => ed.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<EmployeeEducation?> GetEducationByIdAsync(int educationId, CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeEducations
            .FirstOrDefaultAsync(ed => ed.Id == educationId, cancellationToken);

    public async Task CreateEmployeeEducation(EmployeeEducation employeeEducation, CancellationToken cancellationToken = default)
    {
        await _dbContext.EmployeeEducations.AddAsync(employeeEducation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<EmployeeExperience>> GetAllExperiencesByEmployeeIdAsync(Guid employeeId,
                                                                                          CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeExperiences
            .Where(ex => ex.EmployeeId == employeeId)
            .ToListAsync(cancellationToken);

    public async Task<EmployeeExperience?> GetExperienceByIdAsync(int experienceId, CancellationToken cancellationToken = default)
        => await _dbContext.EmployeeExperiences
            .FirstOrDefaultAsync(ex => ex.Id == experienceId, cancellationToken);

    public async Task CreateEmployeeExperience(EmployeeExperience employeeExperience, CancellationToken cancellationToken = default)
    {
        await _dbContext.EmployeeExperiences.AddAsync(employeeExperience, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
