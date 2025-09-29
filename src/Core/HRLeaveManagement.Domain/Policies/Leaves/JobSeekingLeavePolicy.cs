using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class JobSeekingLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "JOB_SEEKING";

    public int? CalculateDays(LeaveEvaluationContext context) => 3;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
