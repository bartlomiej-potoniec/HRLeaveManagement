namespace HRLeaveManagement.Domain.Document;

public interface IEmployeeDocumentNumberUniqueChecker
{
    Task<bool> IsEligibleAsync(string documentNumber, CancellationToken cancellationToken);
}
