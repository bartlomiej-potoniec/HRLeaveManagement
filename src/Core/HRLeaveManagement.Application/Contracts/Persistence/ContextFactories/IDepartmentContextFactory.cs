using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;

public interface IDepartmentContextFactory
{
    DepartmentWithSections AsDepartmentWithSections(Department department);
}
