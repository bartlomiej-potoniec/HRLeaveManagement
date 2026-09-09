using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;

namespace HRLeaveManagement.Application.Contracts.Application;

public interface IEmployeeSubservice
{
    Task<EmployeeContract> CreateEmployeeContract(Employee employee,
                                                  EmployeeContractRequest contractRequest,
                                                  CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EmployeeEducation>> CreateEmployeeEducations(Employee employee,
                                                                    IEnumerable<EmployeeEducationRequest> educationRequests,
                                                                    CancellationToken cancellationToken);

    Task<IReadOnlyList<EmployeeExperience>> CreateEmployeeExperiences(Employee employee,
                                                                      IEnumerable<EmployeeExperienceRequest> experienceRequests,
                                                                      CancellationToken cancellationToken);

    Task UpdateEmployeeContracts(Employee employeeWithDetails,
                                 List<EmployeeContractDetailsRequest> contractDetailsRequests,
                                 CancellationToken cancellationToken = default);
    Task UpdateEmployeeEducations(Employee employeeWithDetails,
                                  List<EmployeeEducationDetailsRequest> educationDetailsRequests,
                                  CancellationToken cancellationToken = default);
    Task UpdateEmployeeExperiences(Employee employeeWithDetails,
                                   List<EmployeeExperienceDetailsRequest> experienceDetailsRequests,
                                   CancellationToken cancellationToken = default);
}
