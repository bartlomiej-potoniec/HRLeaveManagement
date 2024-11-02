namespace HRLeaveManagement.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string Position { get; private set; }
    public string? Responsibilities { get; private set; }

    public int SectionId { get; private set; }
    public Section Section { get; private set; }

    public Guid LeaderId { get; private set; }
    public Employee Leader { get; private set; }

    public List<EmployeeEducation> EmployeeEducations { get; private set; } = [];
    public List<EmployeeContract> EmploymentContracts { get; private set; } = [];
    public List<EmployeeExperience> EmployeeExperiences { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private Employee() {}
    

    // Factory Methods
    public static Employee Create(string position,
                                  string responsibilities,
                                  int sectionId,
                                  Guid leaderId)
        => new()
        {
            Position = position,
            Responsibilities = responsibilities,
            SectionId = sectionId,
            LeaderId = leaderId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(Employee entity,
                              string position,
                              string responsibilities,
                              int sectionId,
                              Guid leaderId)
    {
        entity.Position = position;
        entity.Responsibilities = responsibilities;
        entity.SectionId = sectionId;
        entity.LeaderId = leaderId;
        entity.ModifiedAt = DateTime.UtcNow;
    }
}