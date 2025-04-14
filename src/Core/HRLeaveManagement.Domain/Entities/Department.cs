namespace HRLeaveManagement.Domain.Entities;

public class Department
{
    private readonly List<Section> _sections = [];

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Guid? LeaderId { get; private set; }
    public Employee? Leader { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    public List<Section> Sections => _sections;

    private Department() {}

    #region Domain_Factory_Methods

    public static Department Create(string name,
                                    Guid? leaderId = null,
                                    string? description = null)
    {
        ValidateBaseRules(name, leaderId, description);

        return new()
        {
            Name = name,
            Description = description,
            LeaderId = leaderId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public static void Update(Department department,
                              string name,
                              Guid? leaderId = null,
                              string? description = null)
    {
        ValidateBaseRules(name, leaderId, description);

        department.Name = name;
        department.Description = description;
        department.LeaderId = leaderId;
        department.ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(string name,
                                          Guid? leaderId,
                                          string? description)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name of department cannot be empty");
        }
    }

    #endregion
}
