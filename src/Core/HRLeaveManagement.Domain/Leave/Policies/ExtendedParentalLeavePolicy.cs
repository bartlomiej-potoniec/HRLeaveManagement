using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class ExtendedParentalLeavePolicy : ILeavePolicy
{
    public string Code => "EXTENDED_PARENTAL";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => null;

    public bool IsEligibleFor(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            (context.CurrentContract.ContractType is ContractType.Employment ||
             context.CurrentContract.ContractType is ContractType.B2B ||
             context.CurrentContract.ContractType is ContractType.Other);
}
