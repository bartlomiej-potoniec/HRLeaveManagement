using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class BloodDonationLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "BLOOD_DONATION";

    public int? CalculateDays(LeaveEvaluationContext context) => null;

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}
