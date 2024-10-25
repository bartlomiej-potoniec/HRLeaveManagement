namespace HRLeaveManagement.Domain.Entities;

public class TimeRegister
{
    public int Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateOnly RegisterDate { get; private set; }

    public TimeOnly WorkStartedAt { get; private set; }
    public TimeOnly WorkEndedAt { get; private set; }
    public TimeSpan TotalWorkTime { get; private set; }

    public TimeOnly? BreakStartedAt { get; private set; }
    public TimeOnly? BreakEndedAt { get; private set; }
    public TimeSpan? TotalBreakTime { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private TimeRegister() { }


    // Factory Methods
    public static TimeRegister Create(Guid employeeId,
                                      DateOnly registerDate,
                                      TimeOnly workStartedAt,
                                      TimeOnly workEndedAt,
                                      TimeSpan totalWorkTime,
                                      TimeOnly? breakStartedAt,
                                      TimeOnly? brakeEndedAt,
                                      TimeSpan? totalBreakTime)
        => new()
        {
            EmployeeId = employeeId,
            RegisterDate = registerDate,
            WorkStartedAt = workStartedAt,
            WorkEndedAt = workEndedAt,
            TotalWorkTime = totalWorkTime,
            BreakStartedAt = breakStartedAt,
            BreakEndedAt = brakeEndedAt,
            TotalBreakTime = totalBreakTime,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(TimeRegister entity,
                              Guid employeeId,
                              DateOnly registerDate,
                              TimeOnly workStartedAt,
                              TimeOnly workEndedAt,
                              TimeSpan totalWorkTime,
                              TimeOnly? breakStartedAt,
                              TimeOnly? brakeEndedAt,
                              TimeSpan? totalBreakTime)
    {
        entity.EmployeeId = employeeId;
        entity.RegisterDate = registerDate;
        entity.WorkStartedAt = workStartedAt;
        entity.WorkEndedAt = workEndedAt;
        entity.TotalWorkTime = totalWorkTime;
        entity.BreakStartedAt = breakStartedAt;
        entity.BreakEndedAt = brakeEndedAt;
        entity.TotalBreakTime = totalBreakTime;
        entity.ModifiedAt = DateTime.UtcNow;
    }
}