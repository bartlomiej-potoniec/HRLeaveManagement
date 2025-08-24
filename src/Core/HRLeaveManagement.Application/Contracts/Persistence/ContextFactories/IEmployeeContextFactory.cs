using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;

public interface IEmployeeContextFactory
{
    EmployeeWithAddress AsEmployeeWithAddress(Employee employee);
    EmployeeWithContracts AsEmployeeWithContracts(Employee employee);
    EmployeeWithEducations AsEmployeeWithEducations(Employee employee);
    EmployeeWithExperiences AsEmployeeWithExperiences(Employee employee);
    EmployeeWithRemoteWorkLimits AsEmployeeWithRemoteWorkLimits(Employee employee);
    EmployeeWithLeaveAllocations AsEmployeeWithLeaveAllocations(Employee employee);
    EmployeeWithLeaveRequestsAndAllocation AsEmployeeWithLeaveRequestsAndAllocation(Employee employee);
}
