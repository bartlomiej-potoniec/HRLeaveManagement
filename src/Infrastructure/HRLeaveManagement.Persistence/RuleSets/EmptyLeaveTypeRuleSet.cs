using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Persistence.RuleSets;

// MOVE TO DOMAIN TEST FOLDER!
public sealed class EmptyLeaveTypeRuleSet : ILeaveTypeRuleSet
{
    public static readonly EmptyLeaveTypeRuleSet Instance = new();
    private EmptyLeaveTypeRuleSet() {}

    public Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken)
        => Task.FromResult(true);
}
