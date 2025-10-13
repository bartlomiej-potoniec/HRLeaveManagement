using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.RuleContracts;

public interface ILeaveRequestRuleSet
{
    Task<bool> IsRequestApproverSuperiorOfEmployeeAsync(Employee approver,
                                                        Employee requestingEmployee,
                                                        CancellationToken cancellationToken);

    Task<bool> IsRequesterAllowedToUseLeaveType(Employee requester,
                                                LeaveType leaveType,
                                                CancellationToken cancellationToken);
}
