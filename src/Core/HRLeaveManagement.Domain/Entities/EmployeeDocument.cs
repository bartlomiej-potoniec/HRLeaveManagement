using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public record EmployeeDocumentPayload(string Title, string DocumentNumber, string FileUrl, string? Description = null);

public class EmployeeDocument
{
    public int Id { get; private set; }

    public string Title { get; private set; }
    public string DocumentNumber { get; private set; }
    public string? Description { get; private set; }
    public string FileUrl { get; private set; }

    #region Domain_Navigation_Properties

    public EmployeeContract EmployeeContract { get; private set; }
    public EmployeeEducation EmployeeEducation { get; private set; }
    public EmployeeExperience EmployeeExperience { get; private set; }

    #endregion

    private EmployeeDocument() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="EmployeeDocument"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="title">Title of document</param>
    /// <param name="documentNumber">Document reference number</param>
    /// <param name="description">Document's description</param>
    /// <param name="fileUrl">URL reference to document file</param>
    /// <param name="employeeDocumentRuleSet">Instance of <see cref="IEmployeeDocumentRuleSet"/> for rules checking</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="EmployeeDocument"/></returns>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<EmployeeDocument> CreateSingleAsync(IEmployeeDocumentRuleSet employeeDocumentRuleSet,
                                                                 string title,
                                                                 string documentNumber,
                                                                 string fileUrl,
                                                                 string? description = null,
                                                                 CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(documentNumber, employeeDocumentRuleSet, cancellationToken);

        return new()
        {
            Title = title,
            DocumentNumber = documentNumber,
            Description = description,
            FileUrl = fileUrl,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeDocumentPayloads"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IReadOnlyList<EmployeeDocument>> CreateManyAsync(
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeDocumentPayload> employeeDocumentPayloads,
        CancellationToken cancellationToken = default
    )
    {
        List<EmployeeDocument> employeeDocuments = [];

        foreach (var payload in employeeDocumentPayloads)
        {
            var employeeDocument = await CreateSingleAsync(
                employeeDocumentRuleSet,
                payload.Title,
                payload.DocumentNumber,
                payload.FileUrl,
                payload.Description,
                cancellationToken
            );

            employeeDocuments.Add(employeeDocument);
        }

        return employeeDocuments.AsReadOnly();
    }

    /// <summary>
    /// Updates existing <see cref="EmployeeDocument"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="entity">Given <see cref="EmployeeDocument"/> to be updated</param>
    /// <param name="title">Title of document</param>
    /// <param name="documentNumber">Document reference number</param>
    /// <param name="description">Document's description</param>
    /// <param name="fileUrl">URL reference to document file</param>
    /// <param name="employeeDocumentRuleSet">Instance of <see cref="IEmployeeDocumentRuleSet"/> for rules checking</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="EmployeeDocument"/></returns>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public async Task UpdateAsync(IEmployeeDocumentRuleSet employeeDocumentRuleSet,
                                  string title,
                                  string documentNumber,
                                  string fileUrl,
                                  string? description = null,
                                  CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(documentNumber, employeeDocumentRuleSet, cancellationToken);

        Title = title;
        DocumentNumber = documentNumber;
        Description = description;
        FileUrl = fileUrl;
    }

    private static async Task ValidateBaseRulesAsync(string documentNumber,
                                                     IEmployeeDocumentRuleSet employeeDocumentRuleSet,
                                                     CancellationToken cancellationToken)
    {
        var isEmployeeDocumentNumberUnique = await employeeDocumentRuleSet
            .IsDocumentNumberUniqueAsync(documentNumber, cancellationToken);

        if (!isEmployeeDocumentNumberUnique)
        {
            throw new InvalidOperationException($"Document number: { documentNumber } already exists");
        }
    }

    #endregion
}
