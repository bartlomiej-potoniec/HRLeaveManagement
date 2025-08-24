using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.RuleContracts;

public interface ILeaveRequestRuleSet
{
    Task<bool> IsRequestApproverSuperiorOfEmployeeAsync(Employee approver,
                                                        Employee requestingEmployee,
                                                        CancellationToken cancellationToken);
}
