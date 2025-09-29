using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class MaternityLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "MATERNITY";

    public int? CalculateDays(LeaveEvaluationContext context) => null;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.EmployeeGender is GenderType.Female &&
            (context.CurrentContract.ContractType is ContractType.Employment ||
             context.CurrentContract.ContractType is ContractType.B2B ||
             context.CurrentContract.ContractType is ContractType.Other);
}
