using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class PaternityLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "PATERNITY";

    public int? CalculateDays(LeaveEvaluationContext context) => null;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.EmployeeGender is GenderType.Male &&
            (context.CurrentContract.ContractType is ContractType.Employment ||
             context.CurrentContract.ContractType is ContractType.B2B ||
             context.CurrentContract.ContractType is ContractType.Other);
}
