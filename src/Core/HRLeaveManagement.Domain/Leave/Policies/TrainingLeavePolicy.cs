using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Leave;

namespace HRLeaveManagement.Domain.Leave.Policies;

public class TrainingLeavePolicy : ILeavePolicy
{
    public string Code => "TRAINING_LEAVE";

    public int? CalculateDaysFor(LeaveEvaluationContext context) => 6;

    public bool IsEligibleFor(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
