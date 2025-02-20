using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(Guid id);
    Task CreateWithDetailsAsync(Employee employee,
                                EmployeeContract employeeContract,
                                IEnumerable<EmployeeEducation> employeeEducations,
                                IEnumerable<EmployeeExperience> employeeExperiences);
    Task UpdateBasicInfoAsync(Employee employee);

    Task UpdateWithDetailsAsync(Employee employee,
                                IEnumerable<EmployeeContract> contractsToCreate,
                                IEnumerable<EmployeeEducation> educationsToCreate,
                                IEnumerable<EmployeeExperience> experiencesToCreate,
                                IEnumerable<EmployeeContract> contractsToUpdate,
                                IEnumerable<EmployeeEducation> educationsToUpdate,
                                IEnumerable<EmployeeExperience> experiencesToUpdate,
                                IEnumerable<EmployeeContract> contractsToDelete,
                                IEnumerable<EmployeeEducation> educationsToDelete,
                                IEnumerable<EmployeeExperience> experiencesToDelete);

    Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId);
    Task<EmployeeContract?> GetContractByIdAsync(int contractId);
    Task CreateEmployeeContract(EmployeeContract employeeContract);
}
