using DomainLeaveType = HRLeaveManagement.Domain.Leave.LeaveType.LeaveType;
using HRLeaveManagement.Domain.Leave;
using HRLeaveManagement.Domain.Leave.LeaveType;
using HRLeaveManagement.Domain.Leave.Policies;
using HRLeaveManagement.Domain.Policies.Leaves;

namespace HRLeaveManagement.Application.Features.LeaveAllocation;

public class LeavePolicyFactory
{
    public ILeavePolicy Create(DomainLeaveType leaveType) => leaveType.Rule switch
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
        _                                   => throw new InvalidOperationException($"Unknown leave rule")
    };
}
