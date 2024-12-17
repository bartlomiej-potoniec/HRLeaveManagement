namespace HRLeaveManagement.Domain.Entities;

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

    private Section() {}


    #region Domain_Factory_Methods

    public static Section Create(string name,
                                 int departmentId,
                                 string? description = null)
        => new()
        {
            Name = name,
            Description = description,
            DepartmentId = departmentId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(Section entity,
                              string name,
                              int departmentId,
                              string? description = null)
    {
        entity.Name = name;
        entity.Description = description;
        entity.DepartmentId = departmentId;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
