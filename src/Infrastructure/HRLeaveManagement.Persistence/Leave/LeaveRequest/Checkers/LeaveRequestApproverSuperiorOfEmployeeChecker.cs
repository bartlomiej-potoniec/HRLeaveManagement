using HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Leave.LeaveRequest.Checkers;

public sealed class LeaveRequestApproverSuperiorOfEmployeeChecker(ApplicationDbContext dbContext) 
    : ILeaveRequestApproverSuperiorOfEmployeeChecker
{
    private record EmployeeHierarchyNode(Guid EmployeeId, Guid? LeaderId);

    public async Task<bool> IsEligible(Guid approverId, Guid requestingEmployeeId, CancellationToken cancellationToken)
    {
        var isSuperiorOfEmployee = await CheckIsSuperiorOfEmployee(requestingEmployeeId, approverId);
        return isSuperiorOfEmployee;
    }

    private async Task<bool> CheckIsSuperiorOfEmployee(Guid employeeId, Guid potentialLeaderId)
    {
        var employees = await GetEmployeesAsync();
        var employeesById = employees.ToDictionary(x => x.EmployeeId);

        var currentId = employeeId;
        while (employeesById.TryGetValue(currentId, out var employee))
        {
            if (employee.LeaderId == potentialLeaderId)
            {
                return true;
            }

            if (employee.LeaderId is null)
            {
                return false;
            }

            currentId = employee.LeaderId.Value;
        }

        return false;
    }

    private async Task<IEnumerable<EmployeeHierarchyNode>> GetEmployeesAsync()
        => await dbContext.Employees
            .AsNoTracking()
            .Select(e => new EmployeeHierarchyNode(e.EmployeeId, e.LeaderId));
}
