using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class JobSeekingLeavePolicy : ILeavePolicy
{
    public string Code => "JOB_SEEKING";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => 3;

    public bool IsEligibleFor(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
