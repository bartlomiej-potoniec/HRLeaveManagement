using HRLeaveManagement.Domain.Department.Section;
using DepartmentSection = HRLeaveManagement.Domain.Department.Section.Section;

namespace HRLeaveManagement.Domain.Department;

public sealed record DepartmentPayload(string Name, Guid LeaderId, string? Description = null);

public class Department : IRootEntity
{
    private readonly List<DepartmentSection> _sections = [];

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Guid? LeaderId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    public IReadOnlyList<DepartmentSection> Sections => _sections.AsReadOnly();


    private Department() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="Department"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentNameUniqueChecker"/> for rules checking</param>
    /// <param name="name">Name of department</param>
    /// <param name="leader">Department's leader</param>
    /// <param name="description">Description of department</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<Department> CreateAsync(IDepartmentNameUniqueChecker departmentRuleSet,
                                                     string name,
                                                     Guid leaderId,
                                                     string? description = null,
                                                     CancellationToken cancellationToken = default)
    {
        var IsDepartmentNameUnique = await departmentRuleSet.IsEligible(name, cancellationToken);
        if (!IsDepartmentNameUnique)
        {
            throw new InvalidOperationException($"Department with name '{name}' already exists");
        }

        var department = new Department
        {
            Name = name,
            Description = description,
            LeaderId = leaderId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        return department;
    }

    /// <summary>
    /// Creates the collection of <see cref="Department"/> for given params.
    /// Designates the only way to properly create a collection of object.
    /// </summary>
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentNameUniqueChecker"/> for rules checking</param>
    /// <param name="departmentPayloads">Collection of payload of properties for assigning to instance</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A collection of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<IReadOnlyList<Department>> CreateManyAsync(IDepartmentNameUniqueChecker departmentRuleSet,
                                                                        IEnumerable<DepartmentPayload> departmentPayloads,
                                                                        CancellationToken cancellationToken = default)
    {
        List<Department> departments = [];

        foreach (var payload in departmentPayloads)
        {
            var department = await CreateAsync(
                departmentRuleSet,
                payload.Name,
                payload.LeaderId,
                payload.Description,
                cancellationToken
            );

            departments.Add(department);
        }

        return departments.AsReadOnly();
    }

    /// <summary>
    /// Updates existing <see cref="Department"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentNameUniqueChecker"/> for rules checking</param>
    /// <param name="name">Name of department</param>
    /// <param name="leader">Department's leader</param>
    /// <param name="description">Description of department</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public async Task UpdateAsync(IDepartmentNameUniqueChecker departmentRuleSet,
                                  string name,
                                  Guid leaderId,
                                  string? description = null,
                                  CancellationToken cancellationToken = default)
    {
        var IsDepartmentNameUnique = await departmentRuleSet.IsEligible(name, cancellationToken);
        if (!IsDepartmentNameUnique)
        {
            throw new InvalidOperationException($"Department with name '{name}' already exists");
        }

        Name = name;
        Description = description;
        LeaderId = leaderId;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds <see cref="Section"/> instance to department section list for given params.
    /// Designates the only way to properly create an <see cref="Section"/> object.
    /// </summary>
    /// <param name="name">Name of section of department</param>
    /// <param name="leader">Sections's leader identifier</param>
    /// <param name="description">Description of section</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    internal DepartmentSection AddSingleSection(string name, Guid leader, string? description = null)
    {
        DepartmentSection section = new(name, leader, description, this);
        _sections.Add(section);

        return section;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sectionPayloads"></param>
    /// <returns></returns>
    internal IReadOnlyList<DepartmentSection> AddManySections(IEnumerable<SectionPayload> sectionPayloads)
    {
        List<DepartmentSection> sections = [];

        foreach (var payload in sectionPayloads)
        {
            var section = AddSingleSection(payload.Name, payload.LeaderId, payload.Description);
            sections.Add(section);
        }

        return sections.AsReadOnly();
    }

    #endregion
}