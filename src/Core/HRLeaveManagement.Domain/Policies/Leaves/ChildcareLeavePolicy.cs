using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class ChildcareLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "CHILDCARE";

    public int? CalculateDays(LeaveEvaluationContext context) => 2;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
