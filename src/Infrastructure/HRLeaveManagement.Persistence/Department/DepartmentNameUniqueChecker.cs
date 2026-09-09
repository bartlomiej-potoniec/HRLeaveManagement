using HRLeaveManagement.Domain.Department;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Department;

public sealed class DepartmentNameUniqueChecker(ApplicationDbContext dbContext) : IDepartmentNameUniqueChecker
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private HashSet<string>? _cachedDepartmentNames;

    public async Task<bool> IsEligible(string name, CancellationToken cancellationToken)
    {
        _cachedDepartmentNames ??= new HashSet<string>(
            await GetAllDepartmentsName(cancellationToken),
            StringComparer.OrdinalIgnoreCase);

        return !_cachedDepartmentNames.Contains(name);
    }

    private async Task<IEnumerable<string>> GetAllDepartmentsName(CancellationToken cancellationToken)
        => await _dbContext.Departments
            .Select(dep => dep.Name)
            .ToListAsync(cancellationToken);
}
