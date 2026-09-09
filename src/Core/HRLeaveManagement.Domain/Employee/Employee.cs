using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;
using HRLeaveManagement.Domain.Department.Section;

namespace HRLeaveManagement.Domain.Employee;

public class Employee
{
    private readonly List<EmployeeEducation> _employeeEducations = [];
    private readonly List<EmployeeContract> _employeeContracts = [];
    private readonly List<EmployeeExperience> _employeeExperiences = [];

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } // new
    public string LastName { get; private set; } // new
    public GenderType Gender { get; private set; } // new 

    public string Position { get; private set; }
    public string? Responsibilities { get; private set; }

    public Address.Address Address { get; private set; }

    public int? SectionId { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public IReadOnlyList<EmployeeEducation> EmployeeEducations => _employeeEducations.AsReadOnly();
    public IReadOnlyList<EmployeeContract> EmployeeContracts => _employeeContracts.AsReadOnly();
    public IReadOnlyList<EmployeeExperience> EmployeeExperiences => _employeeExperiences.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    protected Employee() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="Employee"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="position">Actual position of employee</param>
    /// <param name="responsibilities">Responsibilities in the position</param>
    /// <param name="section">Employee's team section identifier</param>
    /// <param name="leader">Employee's leader identifier</param>
    /// <param name="educations">Optional. List of employee education history</param>
    /// <param name="contracts">Optional. List of employee contracts (including actual)</param>
    /// <param name="experiences">Optional. List of employee professional experience history</param>
    /// <returns>A new instance of <see cref="Employee"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    public static Employee Create(string firstName,
                                  string lastName,
                                  string gender,
                                  string position,
                                  string responsibilities,
                                  string residentialAddress,
                                  string registeredAddress,
                                  string? secondaryResidentialAddress = null,
                                  string? remoteWorkAddress = null,
                                  int? sectionId = null,
                                  Employee? leader = null)
    {
        Employee employee = new()
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Gender = MapGender(gender),
            Position = position,
            Responsibilities = responsibilities,
            SectionId = sectionId,
            Leader = leader,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        var address = Domain.Employee.Address.Address.Create(
            employee,
            residentialAddress,
            registeredAddress,
            secondaryResidentialAddress,
            remoteWorkAddress
        );

        employee.Address = address;
        return employee;
    }

    /// <summary>
    /// Updates existing <see cref="Employee"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="position">Actual position of employee</param>
    /// <param name="responsibilities">Responsibilities in the position</param>
    /// <param name="section">Employee's team section identifier</param>
    /// <param name="leader">Employee's leader identifier</param>
    /// <exception cref="ArgumentException">When business rules are violated.</exception>
    internal void Update(string firstName,
                         string lastName,
                         string gender,
                         string position,
                         string responsibilities,
                         string residentialAddress,
                         string registeredAddress,
                         string? secondaryResidentialAddress = null,
                         string? remoteWorkAddress = null,
                         int? sectionId = null,
                         Employee? leader = null)
    {
        if (leader is not null && leader == this)
        {
            throw new ArgumentException("Employee cannot be their own leader");
        }

        Address.Update(residentialAddress, registeredAddress, secondaryResidentialAddress, remoteWorkAddress);

        FirstName = firstName;
        LastName = lastName;
        Gender = MapGender(gender);
        Position = position;
        Responsibilities = responsibilities;
        SectionId = sectionId;
        Leader = leader;
        ModifiedAt = DateTime.UtcNow;
    }

    private static GenderType MapGender(string gender) => gender switch
    {
        "f" => GenderType.Female,
        "m" => GenderType.Male,
        "u" => GenderType.Unspecified,
        _ => throw new NotImplementedException("No gender for given abbreviation"),
    };

    #region EmployeeContract_Subentity_Methods

    /// <summary>
    /// Creates <see cref="EmployeeContract"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="contractType">Employee's contract type</param>
    /// <param name="contractDetails">Optional. Contract details description</param>
    /// <param name="startedAt">Contract start date</param>
    /// <param name="expiredAt">Optional. Contract expiration date</param>
    /// <param name="employeeDocuments">Optional. Documents related to the contract</param>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    internal EmployeeContract AddContract(ContractType contractType,
                                          string? contractDetails,
                                          DateOnly startedAt,
                                          DateOnly? expiredAt = null,
                                          IEnumerable<EmployeeDocument>? employeeDocuments = null)
    {
        EmployeeContract contract = new(
            this,
            contractType,
            contractDetails,
            startedAt,
            expiredAt,
            employeeDocuments);

        _employeeContracts.Add(contract);

        return contract;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeContractPayloads"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<EmployeeContract>> AddManyContractsAsync(
        IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
        IEnumerable<EmployeeContractPayload> employeeContractPayloads,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeContract> employeeContracts = await EmployeeContract.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeContractPayloads,
            cancellationToken);

        _employeeContracts.AddRange(employeeContracts);
        return employeeContracts;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contractToRemove"></param>
    /// <exception cref="ArgumentException"></exception>
    internal void RemoveContract(EmployeeContract contractToRemove)
    {
        var isGivenContractInContractList = _employeeContracts.Contains(contractToRemove);
        if (!isGivenContractInContractList)
        {
            throw new ArgumentException("No such a contract in contract list");
        }

        _employeeContracts.Remove(contractToRemove);
    }

    #endregion

    #region EmployeeExperience_Subentity_Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="experience"></param>
    /// <exception cref="ArgumentException"></exception>
    internal EmployeeExperience AddExperience(ContractType contractType,
                                              string previousCompanyName,
                                              string position,
                                              DateOnly employedFrom,
                                              DateOnly employedTo,
                                              string? experienceDetails = null,
                                              IEnumerable<EmployeeDocument>? employeeDocuments = null)
    {
        EmployeeExperience employeeExperience = new(
            this,
            contractType,
            previousCompanyName,
            position,
            employedFrom,
            employedTo,
            experienceDetails,
            employeeDocuments
        );

        _employeeExperiences.Add(employeeExperience);
        return employeeExperience;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeExperiencePayload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal async Task<IReadOnlyList<EmployeeExperience>> AddManyExperiencesAsync(
        IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
        IEnumerable<EmployeeExperiencePayload> employeeExperiencePayload,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeExperience> employeeExperiences = await EmployeeExperience.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeExperiencePayload,
            cancellationToken        
        );

        _employeeExperiences.AddRange(employeeExperiences);
        return employeeExperiences;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="experienceToRemove"></param>
    /// <exception cref="ArgumentException"></exception>
    internal void RemoveExperience(EmployeeExperience experienceToRemove)
    {
        var isGivenExperienceInEducationList = _employeeExperiences.Contains(experienceToRemove);
        if (!isGivenExperienceInEducationList)
        {
            throw new ArgumentException("No such an experience in contract list");
        }

        _employeeExperiences.Remove(experienceToRemove);
    }

    #endregion

    #region EmployeeEducation_Subentity_Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="education"></param>
    /// <exception cref="ArgumentException"></exception>
    internal EmployeeEducation AddEducation(EducationType educationType,
                                            string institutionName,
                                            DateOnly enrolledAt,
                                            DateOnly? graduatedAt,
                                            string? educationDetails = null,
                                            IEnumerable<EmployeeDocument>? employeeDocuments = null)
    {
        EmployeeEducation employeeEducation = new(
            this,
            educationType,
            institutionName,
            enrolledAt,
            graduatedAt,
            educationDetails,
            employeeDocuments);

        _employeeEducations.Add(employeeEducation);
        return employeeEducation;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDocumentRuleSet"></param>
    /// <param name="employeeEducationPayloads"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal async Task<IReadOnlyList<EmployeeEducation>> AddManyEducationsAsync(
        IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
        IEnumerable<EmployeeEducationPayload> employeeEducationPayloads,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeEducation> employeeEducations = await EmployeeEducation.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeEducationPayloads,
            cancellationToken);

        _employeeEducations.AddRange(employeeEducations);
        return employeeEducations;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="educationToRemove"></param>
    /// <exception cref="ArgumentException"></exception>
    internal void RemoveEducation(EmployeeEducation educationToRemove)
    {
        var isGivenEducationInEducationList = _employeeEducations.Contains(educationToRemove);
        if (!isGivenEducationInEducationList)
        {
            throw new ArgumentException("No such an education in contract list");
        }

        _employeeEducations.Remove(educationToRemove);
    }

    #endregion

    #endregion
}
