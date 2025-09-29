using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Entities;

public sealed record DepartmentPayload(string Name, Employee Leader, string? Description = null);

public class Department : Entity
{
    private readonly List<Section> _sections = [];

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    #region Domain_Navigation_Properties

    public IReadOnlyList<Section> Sections => _sections.AsReadOnly();
    
    #endregion

    private Department() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="Department"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentRuleSet"/> for rules checking</param>
    /// <param name="name">Name of department</param>
    /// <param name="leader">Department's leader</param>
    /// <param name="description">Description of department</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<Department> CreateSingleAsync(IDepartmentRuleSet departmentRuleSet,
                                                           string name,
                                                           Employee leader,
                                                           string? description = null,
                                                           CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(name, departmentRuleSet, cancellationToken);

        var department = new Department
        {
            Name = name,
            Description = description,
            Leader = leader,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

        department.AddEvent(new DepartmentCreated(department));
        return department;
    }

    /// <summary>
    /// Creates the collection of <see cref="Department"/> for given params.
    /// Designates the only way to properly create a collection of object.
    /// </summary>
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentRuleSet"/> for rules checking</param>
    /// <param name="departmentPayloads">Collection of payload of properties for assigning to instance</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A collection of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static async Task<IReadOnlyList<Department>> CreateManyAsync(IDepartmentRuleSet departmentRuleSet,
                                                                        IEnumerable<DepartmentPayload> departmentPayloads,
                                                                        CancellationToken cancellationToken = default)
    {
        List<Department> departments = [];

        foreach (var payload in departmentPayloads)
        {
            var department = await CreateSingleAsync(
                departmentRuleSet,
                payload.Name,
                payload.Leader,
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
    /// <param name="departmentRuleSet">Instance of <see cref="IDepartmentRuleSet"/> for rules checking</param>
    /// <param name="name">Name of department</param>
    /// <param name="leader">Department's leader</param>
    /// <param name="description">Description of department</param>
    /// <param name="cancellationToken">Cancellation Token for operation breaking</param>
    /// <returns>A new instance of <see cref="Department"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public async Task UpdateAsync(IDepartmentRuleSet departmentRuleSet,
                                  string name,
                                  Employee leader,
                                  string? description = null,
                                  CancellationToken cancellationToken = default)
    {
        await ValidateBaseRulesAsync(name, departmentRuleSet, cancellationToken);
        
        Name = name;
        Description = description;
        Leader = leader;
        ModifiedAt = DateTime.UtcNow;

        AddEvent(new DepartmentUpdated(this));
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
    internal Section AddSingleSection(string name, Employee leader, string? description = null)
    {
        Section section = new(name, leader, description, this, new SectionCreationToken());
        _sections.Add(section);

        return section;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sectionPayloads"></param>
    /// <returns></returns>
    internal IReadOnlyList<Section> AddManySections(IEnumerable<SectionPayload> sectionPayloads)
    {
        List<Section> sections = [];

        foreach (var payload in sectionPayloads)
        {
            var section = AddSingleSection(payload.Name, payload.Leader, payload.Description);
            sections.Add(section);
        }

        return sections.AsReadOnly();
    }

    private static async Task ValidateBaseRulesAsync(string name,
                                                     IDepartmentRuleSet departmentRuleSet,
                                                     CancellationToken cancellationToken)
    {
        var IsDepartmentNameUnique = await departmentRuleSet.IsNameUniqueAsync(name, cancellationToken);

        if (!IsDepartmentNameUnique)
        {
            throw new InvalidOperationException($"Department with name '{ name }' already exists");
        }
    }

    private sealed class SectionCreationToken : ISectionCreationToken { internal SectionCreationToken() {} }

    #endregion
}