using HRLeaveManagement.Domain.CreationTokens;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public class Employee : Entity
{
    private readonly List<EmployeeEducation> _employeeEducations = [];
    private readonly List<EmployeeContract> _employeeContracts = [];
    private readonly List<EmployeeExperience> _employeeExperiences = [];

    private readonly List<LeaveRequest> _leaveRequests = [];
    private readonly List<LeaveAllocation> _leaveAllocations = [];

    private readonly List<WorkRequest> _workRequests = [];
    private readonly List<DelegationRequest> _delegationRequests = [];
    private readonly List<ExtraRemoteWorkRequest> _extraRemoteWorkRequests = [];
    private readonly List<OvertimeRequest> _overtimeRequests = [];

    private readonly List<RemoteWorkLimit> _remoteWorkLimits = [];
    private readonly List<TimeRegister> _timeRegisters = [];

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } // new
    public string LastName { get; private set; } // new
    public GenderType Gender { get; private set; } // new 

    public string Position { get; private set; }
    public string? Responsibilities { get; private set; }

    public int? AddressId { get; private set; } // new
    public Address Address { get; private set; } // new

    public int? SectionId { get; private set; }
    public Section? Section { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public IReadOnlyList<EmployeeEducation> EmployeeEducations => _employeeEducations.AsReadOnly();
    public IReadOnlyList<EmployeeContract> EmployeeContracts => _employeeContracts.AsReadOnly();
    public IReadOnlyList<EmployeeExperience> EmployeeExperiences => _employeeExperiences.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    #region Domain_Navigation_Properties
    
    public IReadOnlyList<LeaveRequest> LeaveRequests => _leaveRequests.AsReadOnly();
    public IReadOnlyList<LeaveAllocation> LeaveAllocations => _leaveAllocations.AsReadOnly();
    public IReadOnlyList<WorkRequest> WorkRequests => _workRequests.AsReadOnly();
    public IReadOnlyList<DelegationRequest> DelegationRequests => _delegationRequests.AsReadOnly();
    public IReadOnlyList<ExtraRemoteWorkRequest> ExtraRemoteWorkRequests => _extraRemoteWorkRequests.AsReadOnly();
    public IReadOnlyList<OvertimeRequest> OvertimeRequests => _overtimeRequests.AsReadOnly();
    public IReadOnlyList<RemoteWorkLimit> RemoteWorkLimits => _remoteWorkLimits.AsReadOnly();
    public IReadOnlyList<TimeRegister> TimeRegisters => _timeRegisters.AsReadOnly();

    #endregion

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
    public static Employee Create(Guid userId,
                                  string firstName,
                                  string lastName,
                                  GenderType gender,
                                  string position,
                                  string responsibilities,
                                  string residentialAddress,
                                  string registeredAddress,
                                  string? secondaryResidentialAddress = null,
                                  string? remoteWorkAddress = null,
                                  Section? section = null,
                                  Employee? leader = null)
    {
        Employee employee = new()
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            Position = position,
            Responsibilities = responsibilities,
            Section = section,
            Leader = leader,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        Address address = new(
            employee,
            residentialAddress,
            registeredAddress,
            secondaryResidentialAddress,
            remoteWorkAddress,
            new AddressCreationToken()
        );

        employee.Address = address;

        employee.AddEvent(new EmployeeCreated(employee, userId));
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
                         GenderType gender,
                         string position,
                         string responsibilities,
                         string residentialAddress,
                         string registeredAddress,
                         string? secondaryResidentialAddress = null,
                         string? remoteWorkAddress = null,
                         Section? section = null,
                         Employee? leader = null)
    {
        if (leader is not null && leader == this)
        {
            throw new ArgumentException("Employee cannot be their own leader");
        }

        Address.Update(residentialAddress, registeredAddress, secondaryResidentialAddress, remoteWorkAddress);

        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Position = position;
        Responsibilities = responsibilities;
        Section = section;
        Leader = leader;
        ModifiedAt = DateTime.UtcNow;

        AddEvent(new EmployeeUpdated(this));
    }

    public static GenderType MapGender(string gender) => gender switch
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
            employeeDocuments, 
            new EmployeeContractCreationToken()
        );

        _employeeContracts.Add(contract);
        AddEvent(new EmployeeContractCreated(contract));

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
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeContractPayload> employeeContractPayloads,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeContract> employeeContracts = await EmployeeContract.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeContractPayloads,
            cancellationToken,
            new EmployeeContractCreationToken()
        );

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
            employeeDocuments,
            new EmployeeExperienceCreationToken()
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
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeExperiencePayload> employeeExperiencePayload,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeExperience> employeeExperiences = await EmployeeExperience.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeExperiencePayload,
            cancellationToken,
            new EmployeeExperienceCreationToken()
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
            employeeDocuments,
            new EmployeeEducationCreationToken()
        );

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
        IEmployeeDocumentRuleSet employeeDocumentRuleSet,
        IEnumerable<EmployeeEducationPayload> employeeEducationPayloads,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<EmployeeEducation> employeeEducations = await EmployeeEducation.CreateManyAsync(
            this,
            employeeDocumentRuleSet,
            employeeEducationPayloads,
            cancellationToken,
            new EmployeeEducationCreationToken()
        );

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

    #region LeaveRequest_Entity_Methods

    internal void AddLeaveRequest(LeaveRequest leaveRequest)
    {
        if (leaveRequest.RequestingEmployee != this)
        {
            throw new ArgumentException("This employee is not an owner of given LeaveRequest");
        }

        _leaveRequests.Add(leaveRequest);
    }

    #endregion

    #region LeaveAllocation_Entity_Methods

    internal void AddLeaveAllocation(LeaveAllocation leaveAllocation)
    {
        if (leaveAllocation.Employee != this)
        {
            throw new ArgumentException("This employee is not an owner of given LeaveRequest");
        }

        _leaveAllocations.Add(leaveAllocation);
    }

    #endregion

    #region WorkRequest_Entity_Methods

    /// <summary>
    /// Adds existing <see cref="DelegationRequest"/> instance to employee delegation-request list
    /// </summary>
    /// <param name="delegationRequest"><see cref="DelegationRequest"/> instance to be added</param>
    public void AddDelegationRequest(DelegationRequest delegationRequest) 
    {
        if (delegationRequest.RequestingEmployee != this)
        {
            throw new ArgumentException("This employee is not an owner of given DelegationRequest");
        }

        _delegationRequests.Add(delegationRequest); 
    }

    /// <summary>
    /// Adds existing <see cref="ExtraRemoteWorkRequest"/> instance to employee extra-remote-work-request list
    /// </summary>
    /// <param name="extraRemoteWorkRequest"><see cref="ExtraRemoteWorkRequest"/> instance to be added</param>
    public void AddExtraRemoteWorkRequest(ExtraRemoteWorkRequest extraRemoteWorkRequest)
    {
        if (extraRemoteWorkRequest.RequestingEmployee != this)
        {
            throw new ArgumentException("This employee is not an owner of given ExtraRemoteWorkRequest");
        }

        _extraRemoteWorkRequests.Add(extraRemoteWorkRequest);
    }

    /// <summary>
    /// Adds existing <see cref="OvertimeRequest"/> instance to employee overtime-request list
    /// </summary>
    /// <param name="overtimeRequest"><see cref="OvertimeRequest"/> instance to be added</param>
    public void AddOvertimeRequest(OvertimeRequest overtimeRequest)
    {
        if (overtimeRequest.RequestingEmployee != this)
        {
            throw new ArgumentException("This employee is not an owner of given OvertimeRequest");
        }

        _overtimeRequests.Add(overtimeRequest);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="workRequest"></param>
    public void AddWorkRequest(WorkRequest workRequest)
    {
        if (workRequest.RequestingEmployee != this)
        {
            throw new ArgumentException("This employee is not an owner of given WorkRequest");
        }

        _workRequests.Add(workRequest);
    }

    #endregion

    #region RemoteWorkLimit_Entity_Methods

    internal void AddRemoteWorkLimit(RemoteWorkLimit remoteWorkLimit)
    {
        if (remoteWorkLimit.Employee != this)
        {
            throw new ArgumentException("This employee is not an owner of given RemoteWorkLimit");
        }

        _remoteWorkLimits.Add(remoteWorkLimit);
    }

    #endregion

    #region TimeRegister_Entity_Methods

    public void AddTimeRegister(TimeRegister timeRegister)
    {
        if (timeRegister.Employee != this)
        {
            throw new ArgumentException("This employee is not an owner of given TimeRegister");
        }

        _timeRegisters.Add(timeRegister);
    }

    #endregion

    private sealed class EmployeeContractCreationToken : IEmployeeContractCreationToken { internal EmployeeContractCreationToken() {} }
    private sealed class EmployeeExperienceCreationToken : IEmployeeExperienceCreationToken { internal EmployeeExperienceCreationToken() {} }
    private sealed class EmployeeEducationCreationToken : IEmployeeEducationCreationToken { internal EmployeeEducationCreationToken() {} }
    private sealed class AddressCreationToken : IAddressCreationToken { internal AddressCreationToken() {} }

    #endregion
}
