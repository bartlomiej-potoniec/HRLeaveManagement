using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;

namespace HRLeaveManagement.Persistence.ContextFactories;

public sealed class EmployeeContextFactory : IEmployeeContextFactory
{
    public EmployeeWithAddress AsEmployeeWithAddress(Employee employee)
    {
        EnsureEntityLoaded(employee);
        return new EmployeeWithAddress(employee);
    }

    public EmployeeWithContracts AsEmployeeWithContracts(Employee employee)
    {
        EnsureCollectionLoaded(employee.EmployeeContracts);
        return new EmployeeWithContracts(employee);
    }

    public EmployeeWithEducations AsEmployeeWithEducations(Employee employee)
    {
        EnsureCollectionLoaded(employee.EmployeeEducations);
        return new EmployeeWithEducations(employee);
    }

    public EmployeeWithExperiences AsEmployeeWithExperiences(Employee employee)
    {
        EnsureCollectionLoaded(employee.EmployeeExperiences);
        return new EmployeeWithExperiences(employee);
    }

    public EmployeeWithLeaveAllocations AsEmployeeWithLeaveAllocations(Employee employee)
    {
        EnsureCollectionLoaded(employee.LeaveAllocations);
        return new EmployeeWithLeaveAllocations(employee);
    }

    public EmployeeWithLeaveRequestsAndAllocation AsEmployeeWithLeaveRequestsAndAllocation(Employee employee)
    {
        EnsureEntityLoaded(employee.LeaveAllocations[0]);
        EnsureCollectionLoaded(employee.LeaveRequests);

        return new EmployeeWithLeaveRequestsAndAllocation(employee);
    }

    public EmployeeWithRemoteWorkLimits AsEmployeeWithRemoteWorkLimits(Employee employee)
    {
        EnsureCollectionLoaded(employee.RemoteWorkLimits);
        return new EmployeeWithRemoteWorkLimits(employee);
    }

    private void EnsureCollectionLoaded<TCollection>(IEnumerable<TCollection>? collection)
    {
        if (collection is null)
        {
            throw new AccessViolationException("Missing navigation data");
        }
    }

    private void EnsureEntityLoaded<TEntity>(TEntity? entity)
    {
        if (entity is null)
        {
            throw new AccessViolationException("Missing navigation data");
        }
    }
}
