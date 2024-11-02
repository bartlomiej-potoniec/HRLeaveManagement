namespace HRLeaveManagement.Domain.Entities;

public class RemoteWorkLimit
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public int Year { get; private set; }
    public int AvailableDays { get; private set; }
    public int UsedDays { get; private set; }
    public int RemainingDays { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private RemoteWorkLimit() {}


    // Factory Methods
    public static RemoteWorkLimit Create(Guid employeeId,
                                         int year,
                                         int availableDays)
        => new()
        {
            EmployeeId = employeeId,
            Year = year,
            AvailableDays = availableDays,
            UsedDays = 0,
            RemainingDays = availableDays,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(RemoteWorkLimit entity,
                              Guid employeeId,
                              int year,
                              int availableDays)
    {
        entity.EmployeeId = employeeId;
        entity.Year = year;
        entity.AvailableDays = availableDays;
        entity.RemainingDays = availableDays - entity.UsedDays;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void UpdateDays(RemoteWorkLimit entity,
                                  int requestedDays)
    {
        entity.UsedDays += requestedDays;
        entity.RemainingDays -= requestedDays;
    }
}
