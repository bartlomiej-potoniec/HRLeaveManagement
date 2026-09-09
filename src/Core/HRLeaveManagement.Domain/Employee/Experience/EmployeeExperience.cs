using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Domain.Employee.Experience;

public record EmployeeExperiencePayload(ContractType ContractType,
                                        string PreviousCompanyName,
                                        string Position,
                                        DateOnly EmployedFrom,
                                        DateOnly EmployedTo,
                                        string? ExperienceDetails = null,
                                        IEnumerable<EmployeeDocumentPayload>? EmployeeDocuments = null);

public class EmployeeExperience
{
    private readonly List<EmployeeDocument> _employeeDocuments = [];

    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public ContractType ContractType { get; private set; }

    public string PreviousCompanyName { get; private set; }
    public string Position { get; private set; }
    public string? ExperienceDetails { get; private set; } // new
    public DateOnly EmployedFrom { get; private set; }
    public DateOnly EmployedTo { get; private set; }
    public int TotalEmployment { get; private set; }

    public IReadOnlyList<EmployeeDocument> EmployeeDocuments => _employeeDocuments.AsReadOnly(); // new

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    internal EmployeeExperience(Employee employee,
                                ContractType contractType,
                                string previousCompanyName,
                                string position,
                                DateOnly employedFrom,
                                DateOnly employedTo,
                                string? experienceDetails = default,
                                IEnumerable<EmployeeDocument>? employeeDocuments = default)
    {
        ValidateBaseRules(employedFrom, employedTo);

        Employee = employee;
        ContractType = contractType;
        PreviousCompanyName = previousCompanyName;
        Position = position;
        ExperienceDetails = experienceDetails;
        EmployedFrom = employedFrom;
        EmployedTo = employedTo;
        TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;

        if (employeeDocuments is not null)
        {
            _employeeDocuments.AddRange(employeeDocuments);
        }
    }

    internal static async Task<IReadOnlyList<EmployeeExperience>> CreateManyAsync(Employee employee,
                                                                                  IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
                                                                                  IEnumerable<EmployeeExperiencePayload> employeeExperiencePayloads,
                                                                                  CancellationToken cancellationToken = default)
    {
        List<EmployeeExperience> employeeExperiences = [];

        foreach (var payload in employeeExperiencePayloads)
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

            var employeeExperience = new EmployeeExperience(
                employee,
                payload.ContractType,
                payload.PreviousCompanyName,
                payload.Position,
                payload.EmployedFrom,
                payload.EmployedTo,
                payload.ExperienceDetails,
                documents
            );

            employeeExperiences.Add(employeeExperience);
        }

        return employeeExperiences.AsReadOnly();
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="EmployeeExperience"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="contractType">Employee's contract type</param>
    /// <param name="previousCompanyName">Name of previous company name</param>
    /// <param name="position">Position in previous company name</param>
    /// <param name="employedFrom">Employment start date</param>
    /// <param name="employedTo">Employment end date</param>
    /// <param name="experienceDetails">Description or details of experience</param>
    /// <exception cref="InvalidOperationException">When business rule operations are violated<</exception>
    public void Update(ContractType contractType,
                       string previousCompanyName,
                       string position,
                       DateOnly employedFrom,
                       DateOnly employedTo,
                       string? experienceDetails = null)
    {
        ValidateBaseRules(employedFrom, employedTo);

        ContractType = contractType;
        PreviousCompanyName = previousCompanyName!;
        Position = position;
        ExperienceDetails = experienceDetails;
        EmployedFrom = employedFrom;
        EmployedTo = employedTo;
        TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates existing <see cref="EmployeeExperience"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="contractType">Employee's contract type</param>
    /// <param name="previousCompanyName">Name of previous company name</param>
    /// <param name="position">Position in previous company name</param>
    /// <param name="employedFrom">Employment start date</param>
    /// <param name="employedTo">Employment end date</param>
    /// <param name="experienceDetails">Description or details of experience</param>
    /// <exception cref="InvalidOperationException">When business rule operations are violated<</exception>
    public void Update(ContractType contractType,
                       string previousCompanyName,
                       string position,
                       DateTime employedFrom,
                       DateTime employedTo,
                       string? experienceDetails = null)
        =>
            Update(
                contractType,
                previousCompanyName,
                position,
                DateOnly.FromDateTime(employedFrom),
                DateOnly.FromDateTime(employedTo),
                experienceDetails
            );

    #region EmployeeDocument_Subentity_Methods

    /// <summary>
    /// Adds <see cref="EmployeeDocument"/> instance to employee-contract list.
    /// </summary>
    /// <param name="employeeDocument"><see cref="EmployeeDocument"/> instance</param>
    public void AddDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Add(employeeDocument);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocument"></param>
    public void RemoveDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Remove(employeeDocument);

    #endregion

    private static void ValidateBaseRules(DateOnly employedFrom, DateOnly employedTo)
    {
        if (employedTo < employedFrom)
        {
            throw new InvalidOperationException("Employment end date at previous company must be greater than start date");
        }
    }
    
    #endregion
}
