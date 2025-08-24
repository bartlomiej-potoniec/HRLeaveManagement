using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.RuleSets;

public sealed class EmployeeDocumentRuleSet(ApplicationDbContext dbContext) : IEmployeeDocumentRuleSet
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private HashSet<string>? _cachedDocumentNumbers;

    public async Task<bool> IsDocumentNumberUniqueAsync(string documentNumber, CancellationToken cancellationToken)
    {
        _cachedDocumentNumbers ??= new HashSet<string>(
            await _dbContext.EmployeeDocuments
                .Select(doc => doc.DocumentNumber)
                .ToListAsync(cancellationToken),
            StringComparer.OrdinalIgnoreCase
        );

        return !(_cachedDocumentNumbers.Contains(documentNumber));
    }
}
