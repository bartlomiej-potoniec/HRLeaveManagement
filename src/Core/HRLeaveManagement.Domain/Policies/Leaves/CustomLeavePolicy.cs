using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using System.Linq.Expressions;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class CustomLeavePolicy(int? defaultDays, Func<LeaveEvaluationContext, bool> eligibility) 
    : ILeaveAllocationPolicy
{
    private readonly int? _defaultDays = defaultDays;
    private readonly Func<LeaveEvaluationContext, bool> _eligibility = eligibility;

    public string Code => "CUSTOM";

    public int? CalculateDays(LeaveEvaluationContext context) => _defaultDays;
    public bool IsEligible(LeaveEvaluationContext context) => _eligibility.Invoke(context);
}
