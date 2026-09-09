namespace HRLeaveManagement.Domain.TimeTracking.TimeRegister;

public class TimeRegister
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee.Employee Employee { get; private set; }

    public WorkType WorkType { get; private set; }

    public DateOnly RegisterDate { get; private set; }

    public TimeOnly WorkStartedAt { get; private set; }
    public TimeOnly WorkEndedAt { get; private set; }
    public TimeSpan TotalWorkTime { get; private set; }

    public TimeOnly? BreakStartedAt { get; private set; }
    public TimeOnly? BreakEndedAt { get; private set; }
    public TimeSpan? TotalBreakTime { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private TimeRegister() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Updates existing <see cref="TimeRegister"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="employee">Employee which owns given time register</param>
    /// <param name="registerDate">Date for registering day</param>
    /// <param name="workStartedAt">Time of starting workday</param>
    /// <param name="workEndedAt">Time of ending workday</param>
    /// <param name="breakStartedAt">Optional. Time of starting break in workday</param>
    /// <param name="breakEndedAt">Optional. Time of ending break in workday</param>
    /// <returns>A new instance of <see cref="TimeRegister"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static TimeRegister Create(Employee.Employee employee,
                                      WorkType workType,
                                      DateOnly registerDate,
                                      TimeOnly workStartedAt,
                                      TimeOnly workEndedAt,
                                      TimeOnly? breakStartedAt,
                                      TimeOnly? breakEndedAt,
                                      IEnumerable<TimeRegister> employeeTimeRegisters)
    {
        var isTimeRegisterForDateExist = employeeTimeRegisters
            .Where(register => register.RegisterDate == registerDate) is not null;

        if (isTimeRegisterForDateExist)
        {
            throw new InvalidOperationException("Time register for requested date already exists");
        }

        ValidateBaseRules(workStartedAt, workEndedAt, breakStartedAt, breakEndedAt);

        return new()
        {
            Employee = employee,
            RegisterDate = registerDate,
            WorkStartedAt = workStartedAt,
            WorkEndedAt = workEndedAt,
            TotalWorkTime = workEndedAt - workStartedAt,
            BreakStartedAt = breakStartedAt,
            BreakEndedAt = breakEndedAt,
            TotalBreakTime = breakEndedAt.HasValue && breakEndedAt.HasValue
                ? breakEndedAt - breakStartedAt
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates existing <see cref="TimeRegister"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="entity">Given <see cref="TimeRegister"/> to be updated</param>
    /// <param name="workStartedAt">Time of starting workday</param>
    /// <param name="workEndedAt">Time of ending workday</param>
    /// <param name="breakStartedAt">Optional. Time of starting break in workday</param>
    /// <param name="breakEndedAt">Optional. Time of ending break in workday</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void Update(WorkType workType,
                       TimeOnly workStartedAt,
                       TimeOnly workEndedAt,
                       TimeOnly? breakStartedAt = null,
                       TimeOnly? breakEndedAt = null)
    {
        ValidateBaseRules(workStartedAt, workEndedAt, breakStartedAt, breakEndedAt);

        WorkStartedAt = workStartedAt;
        WorkEndedAt = workEndedAt;
        TotalWorkTime = workEndedAt - workStartedAt;
        BreakStartedAt = breakStartedAt;
        BreakEndedAt = breakEndedAt;
        TotalBreakTime = breakEndedAt.HasValue && breakEndedAt.HasValue
            ? breakEndedAt - breakStartedAt
            : null; 
        ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(TimeOnly workStartedAt,
                                          TimeOnly workEndedAt,
                                          TimeOnly? breakStartedAt,
                                          TimeOnly? breakEndedAt)
    {
        if (workStartedAt >= workEndedAt)
        {
            throw new ArgumentException("Start time of workday must be fewer than end time");
        }

        if (breakStartedAt.HasValue && !breakEndedAt.HasValue ||
            breakEndedAt.HasValue && !breakStartedAt.HasValue)
        {
            throw new InvalidOperationException("Work break must be a complete time range");
        }

        if (breakStartedAt.HasValue && breakEndedAt.HasValue)
        {
            if (breakStartedAt <= workStartedAt || breakEndedAt >= workStartedAt)
            {
                throw new InvalidOperationException("Work break time must be included in work time");
            }
        }
    }

    #endregion
}