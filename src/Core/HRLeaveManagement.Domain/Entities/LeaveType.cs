namespace HRLeaveManagement.Domain.Entities;

public class LeaveType
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public decimal PaidFraction { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private LeaveType() {}


    #region Domain_Factory_Methods
    public static LeaveType Create(string name,
                                   string? description = null,
                                   decimal paidFraction = 1.0M)
        => new()
        {
            Name = name,
            Description = description,
            PaidFraction = paidFraction,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };

    public static void Update(LeaveType entity,
                              string name,
                              string? description = null,
                              decimal paidFraction = 1.0M)
    {
        entity.Name = name;
        entity.Description = description;
        entity.PaidFraction = paidFraction;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
