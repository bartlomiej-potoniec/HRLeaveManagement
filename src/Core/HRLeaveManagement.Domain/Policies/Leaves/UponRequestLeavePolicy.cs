using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class UponRequestLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "UPON_REQUEST";

    public int? CalculateDays(LeaveEvaluationContext context) => 4;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
