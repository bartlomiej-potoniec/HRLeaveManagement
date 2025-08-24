namespace HRLeaveManagement.Domain.RuleContracts;

public interface IEmployeeDocumentRuleSet
{
    Task<bool> IsDocumentNumberUniqueAsync(string documentNumber, CancellationToken cancellationToken);
}
