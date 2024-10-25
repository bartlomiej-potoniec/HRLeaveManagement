namespace HRLeaveManagement.Domain.Entities;

public class Department
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid LeaderId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private Department() { }


    // Factory Methods
    public static Department Create(string name, string? description, Guid leaderId)
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
                              string? description,
                              Guid leaderId)
    {
        entity.Name = name;
        entity.Description = description;
        entity.LeaderId = leaderId;
        entity.ModifiedAt = DateTime.UtcNow;
    }
}
