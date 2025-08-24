namespace HRLeaveManagement.Domain.RuleContracts;

public interface ILeaveTypeRuleSet
{
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken);
}
