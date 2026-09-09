using HRLeaveManagement.Domain.Document;

namespace HRLeaveManagement.Domain.Employee.Contract;

public record EmployeeContractPayload(ContractType ContractType,
                                      string? ContractDetails,
                                      DateOnly StartedAt,
                                      DateOnly? ExpiredAt = null,
                                      IEnumerable<EmployeeDocumentPayload>? EmployeeDocuments = null);

public class EmployeeContract
{
    private readonly List<EmployeeDocument> _employeeDocuments = [];

    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public ContractType ContractType { get; private set; }
    public string? ContractDetails { get; private set; } // new
    
    public DateOnly StartedAt { get; private set; }
    public DateOnly? ExpiredAt { get; private set; }
    public DateOnly? TerminatedAt { get; private set; } // new
    public int? TotalDuration { get; private set; }

    public IReadOnlyList<EmployeeDocument> EmployeeDocuments => _employeeDocuments.AsReadOnly(); // new

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    internal EmployeeContract(Employee employee,
                              ContractType contractType,
                              string? contractDetails,
                              DateOnly startedAt,
                              DateOnly? expiredAt = null,
                              IEnumerable<EmployeeDocument>? employeeDocuments = null) 
    {
        var employeeContracts = employee.EmployeeContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        Employee = employee;
        ContractType = contractType;
        ContractDetails = contractDetails;
        StartedAt = startedAt;
        ExpiredAt = expiredAt;
        TotalDuration = expiredAt.HasValue
            ? expiredAt.Value.DayNumber - startedAt.DayNumber
            : null;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;

        if (employeeDocuments is not null)
        {
            _employeeDocuments.AddRange(employeeDocuments);
        }
    }

    internal static async Task<IReadOnlyList<EmployeeContract>> CreateManyAsync(
        Employee employee,
        IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
        IEnumerable<EmployeeContractPayload> employeeContractPayloads,
        CancellationToken cancellationToken = default)
    {
        List<EmployeeContract> employeeContracts = [];

        foreach (var payload in employeeContractPayloads)
        {
            IReadOnlyList<EmployeeDocument>? documents = null;

            if (payload.EmployeeDocuments is not null)
            {
                documents = await EmployeeDocument.CreateManyAsync(
                    employeeDocumentRuleSet,
                    payload.EmployeeDocuments,
                    cancellationToken
                ); 
            }

            var employeeContract = new EmployeeContract(
                employee,
                payload.ContractType,
                payload.ContractDetails,
                payload.StartedAt,
                payload.ExpiredAt,
                documents
            );

            employeeContracts.Add(employeeContract);
        }

        return employeeContracts.AsReadOnly();
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="EmployeeContract"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="contractType">Employee's contract type</param>
    /// <param name="contractDetails">Optional. Contract details description</param>
    /// <param name="startedAt">Contract start date</param>
    /// <param name="expiredAt">Optional. Contract expiration date</param>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Update(EmployeeWithContracts employee,
                       ContractType contractType,
                       string? contractDetails,
                       DateOnly startedAt,
                       DateOnly? expiredAt = null,
                       IEnumerable<EmployeeDocument>? employeeDocuments = null)
    {
        var employeeContracts = employee.EmployeeContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        ContractType = contractType;
        ContractDetails = contractDetails;
        StartedAt = startedAt;
        ExpiredAt = expiredAt;
        TotalDuration = expiredAt.HasValue
            ? expiredAt.Value.DayNumber - startedAt.DayNumber
            : null;
        ModifiedAt = DateTime.UtcNow;

        if (employeeDocuments is not null)
        {
            _employeeDocuments.Clear();
            _employeeDocuments.AddRange(employeeDocuments);
        }
    }

    /// <summary>
    /// Terminates <see cref="EmployeeContract"/> instance with given params
    /// Designates the only way to properly terminate a contract.
    /// </summary>
    /// <param name="terminationDate">Date when contract was terminated</param>
    /// <param name="expirationDate">Optional. Expiration date. Determines whether contract is notice period observed</param>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Terminate(DateTime todaysDate, DateOnly terminationDate, DateOnly? expirationDate = null)
    {
        var isNoticePeriodObserved = expirationDate is not null;
        var todaysDateOnly = DateOnly.FromDateTime(todaysDate);

        if (terminationDate < todaysDateOnly)
        {
            throw new InvalidOperationException("Contract termination date must be greater than today's date");
        }

        if (isNoticePeriodObserved)
        {
            if (terminationDate > expirationDate)
            {
                throw new InvalidOperationException("Contract termination date must be fewer than expiration date");
            }

            TerminatedAt = terminationDate;
            ExpiredAt = expirationDate;
            TotalDuration = expirationDate.Value.DayNumber - StartedAt.DayNumber;

            return;
        }

        TerminatedAt = terminationDate;
        ExpiredAt = terminationDate;
        TotalDuration = terminationDate.DayNumber - StartedAt.DayNumber;
    }

    #region EmployeeDocument_Entity_Methods

    /// <summary>
    /// Adds <see cref="EmployeeContract"/> instance to employee-contract list.
    /// </summary>
    /// <param name="employeeDocument"><see cref="EmployeeDocument"/> instance</param>
    public void AddDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Add(employeeDocument);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocument"></param>
    public void RemoveDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Remove(employeeDocument);

    #endregion

    private static void ValidateBaseRules(IEnumerable<EmployeeContract> employeeContracts,
                                          ContractType contractType,
                                          DateOnly startedAt,
                                          DateOnly? expiredAt)
    {
        if (expiredAt.HasValue && expiredAt.Value < startedAt)
        {
            throw new InvalidOperationException("Contract expiration date must be greater than start date");
        }

        foreach (var contract in employeeContracts)
        {
            if (!contract.ExpiredAt.HasValue && 
                (!expiredAt.HasValue || expiredAt.Value > contract.StartedAt))
            {
                throw new InvalidOperationException("Cannot define another contract during the indefinite-term contract");
            }

            if (contract.ExpiredAt.HasValue && 
                expiredAt.HasValue &&
                expiredAt.Value > contract.StartedAt && startedAt < contract.ExpiredAt)
            {
                throw new InvalidOperationException("Cannot define another contract during the current contract");
            }

            if (contract.ExpiredAt.HasValue &&
                !expiredAt.HasValue && 
                startedAt < contract.ExpiredAt)
            {
                throw new InvalidOperationException("Cannot define indefinite-term contract during the current contract");
            }
        }
    }

    #endregion
}
