namespace HRLeaveManagement.Domain.RuleContracts;

public interface IDepartmentRuleSet
{
    Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken);
}
