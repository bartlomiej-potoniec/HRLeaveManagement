namespace HRLeaveManagement.Domain.Entities;

public class Employee
{
    private readonly List<EmployeeEducation> _employeeEducations = [];
    private readonly List<EmployeeContract> _employeeContracts = [];
    private readonly List<EmployeeExperience> _employeeExperiences = [];

    public Guid Id { get; private set; }
    public string Position { get; private set; }
    public string? Responsibilities { get; private set; }

    public int? SectionId { get; private set; }
    public Section? Section { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public IEnumerable<EmployeeEducation> EmployeeEducations => _employeeEducations;
    public IEnumerable<EmployeeContract> EmploymentContracts => _employeeContracts;
    public IEnumerable<EmployeeExperience> EmployeeExperiences => _employeeExperiences;

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private Employee() {}

    #region Domain_Factory_Methods

    public static Employee Create(string position,
                                  string responsibilities,
                                  int? sectionId = null,
                                  Guid? leaderId = null,
                                  IEnumerable<EmployeeEducation>? educations = null,
                                  IEnumerable<EmployeeContract>? contracts = null,
                                  IEnumerable<EmployeeExperience>? experiences = null)
    {
        ValidateBaseRules(position, responsibilities, sectionId, leaderId);

        Employee employee = new();

        if (educations is not null)
        {
            employee._employeeEducations.AddRange(educations);
        }

        if (contracts is not null)
        {
            employee._employeeContracts.AddRange(contracts);
        }

        if (experiences is not null)
        {
            employee._employeeExperiences.AddRange(experiences);
        }

        employee.Position = position;
        employee.Responsibilities = responsibilities;
        employee.SectionId = sectionId;
        employee.LeaderId = leaderId;
        employee.CreatedAt = DateTime.UtcNow;
        employee.ModifiedAt = DateTime.UtcNow;

        return employee;
    }

    public static void Update(Employee employee,
                              string position,
                              string responsibilities,
                              int? sectionId = null,
                              Guid? leaderId = null)
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        if (leaderId.HasValue && employee.Id == leaderId)
        {
            throw new InvalidOperationException("Employee cannot be their own leader");
        }

        ValidateBaseRules(position, responsibilities, sectionId, leaderId);

        employee.Position = position;
        employee.Responsibilities = responsibilities;
        employee.SectionId = sectionId;
        employee.LeaderId = leaderId;
        employee.ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(string position,
                                          string responsibilities,
                                          int? sectionId,
                                          Guid? leaderId)
    {
        if (string.IsNullOrEmpty(position))
        {
            throw new ArgumentException("Position for employee cannot be empty");
        }

        if (string.IsNullOrEmpty(responsibilities))
        {
            throw new ArgumentException("Responsibilities for employee cannot be empty");
        }

        if (sectionId is not null && sectionId <= 0)
        {
            throw new ArgumentException("Section ID for employee must be greater than zero");
        }
    }

    #endregion
}