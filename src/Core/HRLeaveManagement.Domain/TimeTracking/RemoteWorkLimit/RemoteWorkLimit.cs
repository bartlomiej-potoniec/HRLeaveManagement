namespace HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;

public class RemoteWorkLimit : IRootEntity
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee.Employee Employee { get; private set; }

    public int Year { get; private set; }
    public int AvailableDays { get; private set; }
    public int UsedDays { get; private set; }
    public int RemainingDays { get; private set; }
    public DateTime CreatedAt { get; private set; } 
    public DateTime ModifiedAt { get; private set; }

    private RemoteWorkLimit() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="RemoteWorkLimit"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="employee">Employee which owns given time remote work limit</param>
    /// <param name="currentYear">Current year in calendar</param>
    /// <param name="year">Year of remote work limit</param>
    /// <param name="availableDays">Available days in particular year</param>
    /// <returns>A new instance of <see cref="RemoteWorkLimit"/></returns>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public static RemoteWorkLimit Create(Employee.Employee employee,
                                         int currentYear,
                                         int year,
                                         int availableDays,
                                         IEnumerable<RemoteWorkLimit> employeeWorkLimits)
    {
        var isRemoteWorkLimitForYearExist = employeeWorkLimits
            .Where(limit => limit.Year == year) is not null;

        if (isRemoteWorkLimitForYearExist)
        {
            throw new InvalidOperationException("Remote work limit for requested year already exists");
        }

        ValidateBaseRules(currentYear, year, availableDays);

        return new()
        {
            Employee = employee,
            Year = year,
            AvailableDays = availableDays,
            UsedDays = 0,
            RemainingDays = availableDays,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates existing <see cref="RemoteWorkLimit"/> instance with given params.
    /// Designates the only way to properly modify an object.
    /// </summary>
    /// <param name="currentYear">Current year in calendar</param>
    /// <param name="year">Year of remote work limit</param>
    /// <param name="availableDays">Available days in particular year</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>

    public void Update(int currentYear, int year, int availableDays)
    {
        ValidateBaseRules(currentYear, year, availableDays);

        if (availableDays < UsedDays)
        {
            throw new InvalidOperationException("Given available days value may not be fewer than currently used days");
        }

        Year = year;
        AvailableDays = availableDays;
        RemainingDays = availableDays - UsedDays;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates days of remote work limit
    /// </summary>
    /// <param name="requestedDays">Requested days count to be used within the available days</param>
    /// <exception cref="ArgumentException">When business rules are violated</exception>
    /// <exception cref="InvalidOperationException">When business rule operations are violated</exception>
    public void UseDays(int requestedDays)
    {
        if (requestedDays < 0)
        {
            throw new ArgumentException("Requested days in remote work limit must be a positive number");
        }

        if (requestedDays > RemainingDays)
        {
            throw new InvalidOperationException("Requested days in remote work limit must be fewer than available days");
        }

        UsedDays += requestedDays;
        RemainingDays -= requestedDays;
    }

    private static void ValidateBaseRules(int currentYear, int year, int availableDays)
    {
        var daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

        if (year < currentYear)
        {
            throw new InvalidOperationException("Changes on remote work limits may only be performed for current and future year");
        }

        if (availableDays < 0)
        {
            throw new ArgumentException("Available days in remote work limit must be a positive number");
        }

        if (availableDays > daysInYear)
        {
            throw new InvalidOperationException("Available days in remote work limit must be fewer than days in requested year");
        }
    }

    #endregion
}
