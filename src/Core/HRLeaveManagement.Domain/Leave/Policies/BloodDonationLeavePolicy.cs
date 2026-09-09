using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Domain.Leave.Policies;

public class BloodDonationLeavePolicy : ILeavePolicy
{
    public string Code => "BLOOD_DONATION";

    public int? CalculateDaysFor(Employee.Employee context) => null;

    public bool IsEligibleFor(Employee.Employee context)
        => context.CurrentContract is not null &&
            context.CurrentContract.ContractType is ContractType.Employment;
}