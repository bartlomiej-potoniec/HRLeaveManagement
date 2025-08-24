using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.RuleSets;

public sealed class DepartmentRuleSet(ApplicationDbContext dbContext) : IDepartmentRuleSet
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private HashSet<string>? _cachedDepartmentNames;

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken)
    {
        _cachedDepartmentNames ??= new HashSet<string>(
            await _dbContext.Departments
                .Select(dep => dep.Name)
                .ToListAsync(cancellationToken),
            StringComparer.OrdinalIgnoreCase
        );

        return !(_cachedDepartmentNames.Contains(name));
    }
}
