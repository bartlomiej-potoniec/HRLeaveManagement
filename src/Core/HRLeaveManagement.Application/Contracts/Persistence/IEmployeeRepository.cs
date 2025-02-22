using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateWithDetailsAsync(Employee employee,
                                EmployeeContract employeeContract,
                                IEnumerable<EmployeeEducation> employeeEducations,
                                IEnumerable<EmployeeExperience> employeeExperiences,
                                CancellationToken cancellationToken = default);
    Task UpdateBasicInfoAsync(Employee employee, CancellationToken cancellationToken = default);

    Task UpdateWithDetailsAsync(Employee employee,
                                IEnumerable<EmployeeContract> contractsToCreate,
                                IEnumerable<EmployeeEducation> educationsToCreate,
                                IEnumerable<EmployeeExperience> experiencesToCreate,
                                IEnumerable<EmployeeContract> contractsToUpdate,
                                IEnumerable<EmployeeEducation> educationsToUpdate,
                                IEnumerable<EmployeeExperience> experiencesToUpdate,
                                IEnumerable<EmployeeContract> contractsToDelete,
                                IEnumerable<EmployeeEducation> educationsToDelete,
                                IEnumerable<EmployeeExperience> experiencesToDelete,
                                CancellationToken cancellationToken = default);

    Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId,
                                                                         CancellationToken cancellationToken = default);
    Task<EmployeeContract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken = default);
    Task CreateEmployeeContract(EmployeeContract employeeContract, CancellationToken cancellationToken = default);
}
