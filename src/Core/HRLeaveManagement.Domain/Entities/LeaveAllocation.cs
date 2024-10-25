namespace HRLeaveManagement.Domain.Entities;

public class LeaveAllocation
{
    public int Id { get; private set; }
    public Guid EmployeeId { get; private set; }

    public int LeaveTypeId { get; private set; }
    public LeaveType? LeaveType { get; private set; }

    public int Year { get; private set; }

    public int? AvailableDays { get; private set; }
    public int? UsedDays { get; private set; }
    public int? RemainingDays { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private LeaveAllocation() { }


    // Factory Methods
    public static LeaveAllocation Create(Guid employeeId,
                                         int leaveTypeId,
                                         int year,
                                         int? availableDays)
        => new()
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            Year = year,
            AvailableDays = availableDays,
            UsedDays = 0,
            RemainingDays = availableDays,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(LeaveAllocation entity,
                              Guid employeeId,
                              int leaveTypeId,
                              int year,
                              int? availableDays)
    {
        entity.EmployeeId = employeeId;
        entity.LeaveTypeId = leaveTypeId;
        entity.Year = year;
        entity.AvailableDays = availableDays;
        entity.RemainingDays = availableDays - entity.UsedDays;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void UpdateDays(LeaveAllocation entity,
                                  int requestedDays)
    {
        entity.AvailableDays -= requestedDays;
        entity.UsedDays += requestedDays;
        entity.RemainingDays = entity.AvailableDays - entity.UsedDays;
    }
}
