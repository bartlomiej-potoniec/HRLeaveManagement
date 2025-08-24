using HRLeaveManagement.Domain.BoundedEntities;

namespace HRLeaveManagement.Domain.Entities;

public sealed record SectionPayload(string Name, Employee Leader, string? Description = null);

public class Section
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public int DepartmentId { get; private set; }
    public Department Department { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    internal Section(string name,
                     Employee leader,
                     string? description,
                     Department department,
                     ISectionCreationToken token) 
    {
        if (token is null)
        {
            throw new AccessViolationException("Attempted to create Section without proper domain context");
        }

        ValidateBaseRules(name, department);

        Name = name;
        Description = description;
        Department = department;
        Leader = leader;
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="Section"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="entity">Given <see cref="Section"/> to be updated</param>
    /// <param name="name">Name of section of department</param>
    /// <param name="department">Superior department for the section</param>
    /// <param name="leader">Sections's leader identifier</param>
    /// <param name="description">Description of section</param>
    /// <returns>A new instance of <see cref="Section"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Update(DepartmentWithSections department,
                       string name,
                       Employee leader,
                       string? description = null)
    {
        ValidateBaseRules(name, department.Department);

        Name = name;
        Description = description;
        Leader = leader;
        ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(string name, Department department)
    {
        foreach (var section in department.Sections)
        {
            if (name.Equals(section.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Section with name '{ name }' for department: { department.Name } already exists"
                );
            }
        }
    }

    #endregion
}
