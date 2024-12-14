using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Repositories;

public class EmployeeRepository(ApplicationDbContext dbContext,
                                IAppLogger<EmployeeRepository> logger) 
    : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IAppLogger<EmployeeRepository> _logger = logger;

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _dbContext.Employees
            .Include(e => e.Section)
                .ThenInclude(s => s.Department)
            .Include(e => e.Leader)
            .Include(e => e.EmploymentContracts)
            .ToListAsync();
         
    public async Task<Guid> CreateWithDetailsAsync(Employee employee,
                                                   EmployeeContract employeeContract,
                                                   IEnumerable<EmployeeEducation> employeeEducations,
                                                   IEnumerable<EmployeeExperience> employeeExperiences)
    {
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.EmployeeContracts.AddAsync(employeeContract);
        await _dbContext.EmployeeEducations.AddRangeAsync(employeeEducations);
        await _dbContext.EmployeeExperiences.AddRangeAsync(employeeExperiences);

        await _dbContext.SaveChangesAsync();
        return employee.Id; 
    }
}
