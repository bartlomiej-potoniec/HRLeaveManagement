using HRLeaveManagement.Domain.CreationTokens;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public record EmployeeEducationPayload(EducationType EducationType,
                                       string InstitutionName,
                                       DateOnly EnrolledAt,
                                       DateOnly? GraduatedAt,
                                       string? EducationDetails = null,
                                       IEnumerable<EmployeeDocumentPayload>? EmployeeDocuments = null);

public class EmployeeEducation
{
    private readonly List<EmployeeDocument> _employeeDocuments = [];

    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public EducationType EducationType { get; private set; }
    public string InstitutionName { get; private set; } // new
    public string? EducationDetails { get; private set; }

    public DateOnly EnrolledAt { get; private set; }
    public DateOnly? GraduatedAt { get; private set; }
    public int? TotalDuration { get; private set; } // new

    public IReadOnlyList<EmployeeDocument> EmployeeDocuments => _employeeDocuments.AsReadOnly(); // new

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    internal EmployeeEducation(Employee employee,
                               EducationType educationType,
                               string institutionName,
                               DateOnly enrolledAt,
                               DateOnly? graduatedAt,
                               string? educationDetails = null,
                               IEnumerable<EmployeeDocument>? employeeDocuments = null,
                               IEmployeeEducationCreationToken creationToken = default)
    {
        if (creationToken is null)
        {
            throw new AccessViolationException("Attempted to create EmployeeEducation without proper domain context");
        }

        ValidateBaseRules(enrolledAt, graduatedAt);

        Employee = employee;
        EducationType = educationType;
        InstitutionName = institutionName;
        EducationDetails = educationDetails;
        EnrolledAt = enrolledAt;
        GraduatedAt = graduatedAt;
        TotalDuration = graduatedAt is not null
            ? graduatedAt.Value.DayNumber - enrolledAt.DayNumber
            : null;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;

        if (employeeDocuments is not null)
        {
            _employeeDocuments.AddRange(employeeDocuments);
        }
    }

    internal static async Task<IReadOnlyList<EmployeeEducation>> CreateManyAsync(
        Employee employee,
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeEducationPayload> employeeEducationPayloads,
        CancellationToken cancellationToken = default,
        IEmployeeEducationCreationToken creationToken = default
    )
    {
        List<EmployeeEducation> employeeEducations = [];

        foreach (var payload in employeeEducationPayloads)
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

            var employeeContract = new EmployeeEducation(
                employee,
                payload.EducationType,
                payload.InstitutionName,
                payload.EnrolledAt,
                payload.GraduatedAt,
                payload.EducationDetails,
                documents,
                creationToken
            );

            employeeEducations.Add(employeeContract);
        }

        return employeeEducations.AsReadOnly();
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="EmployeeEducation"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="educationType">Employee's education type</param>
    /// <param name="institutionName">Education institution name</param>
    /// <param name="enrolledAt">Education start date</param>
    /// <param name="graduatedAt">Optional. Education end date</param>
    /// <param name="educationDetails">Optional. Description or details</param>
    public void Update(EducationType educationType,
                       string institutionName,
                       DateOnly enrolledAt,
                       DateOnly? graduatedAt,
                       string? educationDetails = null)
    {
        ValidateBaseRules(enrolledAt, graduatedAt);

        EducationType = educationType;
        EducationDetails = educationDetails;
        InstitutionName = institutionName;
        EnrolledAt = enrolledAt;
        GraduatedAt = graduatedAt;
        TotalDuration = graduatedAt is not null
            ? graduatedAt.Value.DayNumber - enrolledAt.DayNumber
            : null;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates existing <see cref="EmployeeEducation"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="educationType">Employee's education type</param>
    /// <param name="institutionName">Education institution name</param>
    /// <param name="enrolledAt">Education start date</param>
    /// <param name="graduatedAt">Optional. Education end date</param>
    /// <param name="educationDetails">Optional. Description or details</param>
    public void Update(EducationType educationType,
                       string institutionName,
                       DateTime enrolledAt,
                       DateTime? graduatedAt,
                       string? educationDetails)
        => Update(
            educationType,
            institutionName,
            DateOnly.FromDateTime(enrolledAt),
            graduatedAt.HasValue ? DateOnly.FromDateTime(graduatedAt.Value) : null,
            educationDetails
        );

    #region EmployeeDocument_Subentity_Methods

    /// <summary>
    /// Adds <see cref="EmployeeDocument"/> instance to employee-education list.
    /// </summary>
    /// <param name="employeeDocument"><see cref="EmployeeDocument"/> instance</param>
    public void AddDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Add(employeeDocument);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocument"></param>
    public void RemoveDocument(EmployeeDocument employeeDocument) => _employeeDocuments.Remove(employeeDocument);

    #endregion

    private static void ValidateBaseRules(DateOnly enrolledAt, DateOnly? graduatedAt)
    {
        if (graduatedAt.HasValue && graduatedAt < enrolledAt)
        {
            throw new InvalidOperationException("Education graduation date must be greater than enroll date");
        }
    }

    #endregion
}