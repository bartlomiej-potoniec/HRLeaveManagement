using HRLeaveManagement.Domain.Document;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.Document;

public sealed class EmployeeDocumentNumberUniqueChecker(ApplicationDbContext dbContext) : IEmployeeDocumentNumberUniqueChecker
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private HashSet<string>? _cachedDocumentNumbers;

    public async Task<bool> IsEligibleAsync(string documentNumber, CancellationToken cancellationToken)
    {
        _cachedDocumentNumbers ??= new HashSet<string>(
            await GetAllDocumentNumbers(cancellationToken),
            StringComparer.OrdinalIgnoreCase
        );

        return !_cachedDocumentNumbers.Contains(documentNumber);
    }

    private async Task<List<string>> GetAllDocumentNumbers(CancellationToken cancellationToken)
        => await _dbContext.EmployeeDocuments
            .Select(doc => doc.DocumentNumber)
            .ToListAsync(cancellationToken);
}
