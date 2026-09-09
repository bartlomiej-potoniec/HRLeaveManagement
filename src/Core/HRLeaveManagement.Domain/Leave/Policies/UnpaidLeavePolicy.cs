using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class UnpaidLeavePolicy : ILeavePolicy
{
    public string Code => "UNPAID";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => null;

    public bool IsEligibleFor(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
