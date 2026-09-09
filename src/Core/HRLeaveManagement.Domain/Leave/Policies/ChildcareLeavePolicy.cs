using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class ChildcareLeavePolicy : ILeavePolicy
{
    public string Code => "CHILDCARE";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => 2;

    public bool IsEligibleFor(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
