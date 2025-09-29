using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class TrainingLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "TRAINING_LEAVE";

    public int? CalculateDays(LeaveEvaluationContext context) => 6;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
