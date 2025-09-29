using DomainLeaveType = HRLeaveManagement.Domain.Entities.LeaveType;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Policies.Leaves;

namespace HRLeaveManagement.Application.Features.LeaveAllocation;

public class LeavePolicyFactory
{
    public ILeaveAllocationPolicy Create(DomainLeaveType leaveType) => leaveType.Rule switch
    {
        LeaveRuleType.AnnualLeave           => new AnnualLeavePolicy(),
        LeaveRuleType.UponRequestLeave      => new UponRequestLeavePolicy(),
        LeaveRuleType.SpecialLeave          => new SpecialLeavePolicy(),
        LeaveRuleType.UnpaidLeave           => new UnpaidLeavePolicy(),
        LeaveRuleType.MaternityLeave        => new MaternityLeavePolicy(),
        LeaveRuleType.PaternityLeave        => new PaternityLeavePolicy(),
        LeaveRuleType.ParentalLeave         => new ParentalLeavePolicy(),
        LeaveRuleType.ExtendedParentalLeave => new ExtendedParentalLeavePolicy(),
        LeaveRuleType.ChildcareLeave        => new ChildcareLeavePolicy(),
        LeaveRuleType.CarersLeave           => new CarersLeavePolicy(),
        LeaveRuleType.UrgentMattersLeave    => new UrgentMattersLeavePolicy(),
        LeaveRuleType.JobSeekingLeave       => new JobSeekingLeavePolicy(),
        LeaveRuleType.TrainingLeave         => new TrainingLeavePolicy(),
        LeaveRuleType.BloodDonationLeave    => new BloodDonationLeavePolicy(),
        LeaveRuleType.CustomLeave           => new CustomLeavePolicy(leaveType.DefaultDays, c => false),
        _                                   => throw new InvalidOperationException($"Unknown leave rule")
    };
}
