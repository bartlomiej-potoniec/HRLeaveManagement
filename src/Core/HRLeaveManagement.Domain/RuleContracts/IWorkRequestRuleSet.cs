using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.RuleContracts;

public interface IWorkRequestRuleSet
{
    Task<bool> IsRequestApproverSuperiorOfEmployeeAsync(Employee approver,
                                                        Employee requestingEmployee,
                                                        CancellationToken cancellationToken);
}
