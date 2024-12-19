namespace HRLeaveManagement.Domain.Entities;

public class LeaveAllocation
{
    public int Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public int LeaveTypeId { get; private set; }
    public LeaveType LeaveType { get; private set; }

    public int Year { get; private set; }

    public int? AvailableDays { get; private set; }
    public int? UsedDays { get; private set; }
    public int? RemainingDays { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private LeaveAllocation() { }


    #region Domain_Factory_Methods

    public static LeaveAllocation Create(Guid employeeId,
                                         int leaveTypeId,
                                         int year,
                                         int? availableDays = null)
        => new()
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            Year = year,
            AvailableDays = availableDays is not null ? availableDays : null,
            UsedDays = availableDays is not null ? 0 : null,
            RemainingDays = availableDays is not null ? availableDays : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(LeaveAllocation entity, int? availableDays = null)
    {
        if (entity.AvailableDays is null &&
            entity.UsedDays is null &&
            entity.RemainingDays is null &&
            availableDays is not null)
        {
            entity.UsedDays = 0;
            entity.RemainingDays = availableDays;
        }

        if (entity.AvailableDays is not null &&
            entity.UsedDays is not null &&
            entity.RemainingDays is not null &&
            availableDays is not null)
        {
            entity.RemainingDays = availableDays - entity.UsedDays;
        }

        if (entity.AvailableDays is not null &&
            entity.UsedDays is not null &&
            entity.RemainingDays is not null &&
            availableDays is null)
        {
            entity.UsedDays = null;
            entity.RemainingDays = null;
        }

        entity.AvailableDays = availableDays;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void UseDays(LeaveAllocation entity,
                               int? requestedDays = null)
    {
        if (entity.AvailableDays is not null &&
            entity.UsedDays is not null &&
            entity.RemainingDays is not null &&
            requestedDays is not null)
        {
            entity.UsedDays += requestedDays;
            entity.RemainingDays -= requestedDays;
        }
    }

    #endregion
}
