using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class UnpaidLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "UNPAID";

    public int? CalculateDays(LeaveEvaluationContext context) => null;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
