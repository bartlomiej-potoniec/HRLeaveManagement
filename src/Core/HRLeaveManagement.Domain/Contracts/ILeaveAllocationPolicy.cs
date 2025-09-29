using HRLeaveManagement.Domain.BoundedEntities;

namespace HRLeaveManagement.Domain.Contracts;

public interface ILeaveAllocationPolicy
{
    string Code { get; }
    bool IsEligible(LeaveEvaluationContext context);
    int? CalculateDays(LeaveEvaluationContext context);
}
