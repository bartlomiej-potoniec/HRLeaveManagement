namespace HRLeaveManagement.Domain.Entities;

public class Department
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    public List<Section> Sections { get; private set; } = [];

    private Department() {}


    #region Domain_Factory_Methods

    public static Department Create(string name, Guid? leaderId, string? description = null)
        => new()
        {
            Name = name,
            Description = description,
            LeaderId = leaderId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(Department entity,
                              string name,
                              Guid? leaderId,
                              string? description = null)
    {
        entity.Name = name;
        entity.Description = description;
        entity.LeaderId = leaderId;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
