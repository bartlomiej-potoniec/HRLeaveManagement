using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;

namespace HRLeaveManagement.Application.Contracts.Persistence.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetWithContractsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetWithRemoteWorkLimitsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetWithLeaveAllocationsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetWithLeaveRequestsAndAllocationByIdAsync(Guid id, int leaveTypeId, int currentYear, CancellationToken cancellationToken = default);

    Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
    Task CreateWithDetailsAsync(Employee employee,
                                EmployeeContract employeeContract,
                                IEnumerable<EmployeeEducation> employeeEducations,
                                IEnumerable<EmployeeExperience> employeeExperiences,
                                CancellationToken cancellationToken = default);
    Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId,
                                                                         CancellationToken cancellationToken = default);
    Task<EmployeeContract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken = default);
    Task CreateEmployeeContract(EmployeeContract employeeContract, CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeEducation>> GetAllEducationsByEmployeeIdAsync(Guid employeeId,
                                                                           CancellationToken cancellationToken = default);
    Task<EmployeeEducation?> GetEducationByIdAsync(int educationId, CancellationToken cancellationToken = default);
    Task CreateEmployeeEducation(EmployeeEducation employeeEducation, CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeExperience>> GetAllExperiencesByEmployeeIdAsync(Guid employeeId,
                                                                             CancellationToken cancellationToken = default);
    Task<EmployeeExperience?> GetExperienceByIdAsync(int experienceId, CancellationToken cancellationToken = default);
    Task CreateEmployeeExperience(EmployeeExperience employeeExperience, CancellationToken cancellationToken = default);
}
