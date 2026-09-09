namespace HRLeaveManagement.Domain.Department.Section;

public sealed record SectionPayload(string Name, Guid LeaderId, string? Description = null);

public class Section : IEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public int DepartmentId { get; private set; }
    public Department Department { get; private set; }

    public Guid? LeaderId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    internal Section(string name,
                     Guid leaderId,
                     string? description,
                     Department department) 
    {
        var isAnySectionNameDuplicatesInCollection = department.Sections
            .Any(section => section.Name == name);
        if (isAnySectionNameDuplicatesInCollection)
        {
            throw new InvalidOperationException($"Section with name '{name}' for department: {department.Name} already exists");
        }

        Name = name;
        Description = description;
        Department = department;
        LeaderId = leaderId;
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
    public void Update(Department department,
                       string name,
                       Guid leaderId,
                       string? description = null)
    {
        var isAnySectionNameDuplicatesInCollection = department.Sections
            .Any(section => section.Name == name);
        if (isAnySectionNameDuplicatesInCollection)
        {
            throw new InvalidOperationException($"Section with name '{name}' for department: {department.Name} already exists");
        }

        Name = name;
        Description = description;
        LeaderId = leaderId;
        ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
