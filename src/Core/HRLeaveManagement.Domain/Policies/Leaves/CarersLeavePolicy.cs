using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class CarersLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "CARERS";

    public int? CalculateDays(LeaveEvaluationContext context) => 5;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
