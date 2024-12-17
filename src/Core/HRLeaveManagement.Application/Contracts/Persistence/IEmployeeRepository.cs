using HRLeaveManagement.Domain.Entities;
using MediatR;

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

    Task<IEnumerable<EmployeeContract>> GetAllContractsByEmployeeIdAsync(Guid employeeId);
    Task<EmployeeContract?> GetContractByIdAsync(int contractId);
    Task CreateEmployeeContract(EmployeeContract employeeContract);
}
