namespace HRLeaveManagement.Domain.Leave.LeaveAllocation;

public class LeaveAllocation : IRootEntity
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee.Employee Employee { get; private set; }

    public int LeaveTypeId { get; private set; }
    public LeaveType.LeaveType LeaveType { get; private set; }

    public int Year { get; private set; }

    public int? AvailableDays { get; private set; }
    public int? UsedDays { get; private set; }
    public int? RemainingDays { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private LeaveAllocation() {}

    #region Domain_Factory_Methods

    /// <summary>
    /// Creates <see cref="LeaveAllocation"/> instance for given params.
    /// Designates the only way to properly create an object.
    /// </summary>
    /// <param name="employee">Employee with leave allocation list for which new allocation is intended to be created</param>
    /// <param name="leaveType">Leave type of particular allocation</param>
    /// <param name="year">Year for new leave allocation</param>
    /// <param name="availableDays">Available days count for specific leave-type allocation</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static LeaveAllocation Create(ILeavePolicy policy,
                                         Employee.Employee employee,
                                         LeaveType.LeaveType leaveType,
                                         int year,
                                         int? availableDays,
                                         IEnumerable<LeaveAllocation> employeeLeaveAllocations)
    {
        if (!policy.IsEligibleFor(employee))
        {
            throw new InvalidOperationException($"Employee is not allowed to allocate leave type: { leaveType.Name }");
        }

        var isLeaveAllocationForYearExist = employeeLeaveAllocations
            .Where(la => la.LeaveType == leaveType && la.Year == year) is not null;
        if (isLeaveAllocationForYearExist)
        {
            throw new InvalidOperationException("Leave allocation for given year already exists");
        }

        return new()
        {
            Employee = employee,
            LeaveType = leaveType,
            Year = year,
            AvailableDays = availableDays is not null ? availableDays : null,
            UsedDays = availableDays is not null ? 0 : null,
            RemainingDays = availableDays is not null ? availableDays : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates specific leave-allocation days
    /// </summary>
    /// <param name="availableDays">New pool of leave available days</param>
    public void UpdateDays(int? availableDays = null)
    {
        if (AvailableDays is null &&
            UsedDays is null &&
            RemainingDays is null &&
            availableDays is not null)
        {
            UsedDays = 0;
            RemainingDays = availableDays;
        }

        if (AvailableDays is not null &&
            UsedDays is not null &&
            RemainingDays is not null &&
            availableDays is not null)
        {
            RemainingDays = availableDays - UsedDays;
        }

        if (AvailableDays is not null &&
            UsedDays is not null &&
            RemainingDays is not null &&
            availableDays is null)
        {
            UsedDays = null;
            RemainingDays = null;
        }

        AvailableDays = availableDays;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Uses specific leave-allocation days on request
    /// </summary>
    /// <param name="requestedDays">Requested leave days to be used</param>
    public void UseDays(int? requestedDays = null)
    {
        if (AvailableDays is not null &&
            UsedDays is not null &&
            RemainingDays is not null &&
            requestedDays is not null)
        {
            UsedDays += requestedDays;
            RemainingDays -= requestedDays;
        }
    }

    /// <summary>
    /// Returns specific leave-allocation days when cancel request after approving
    /// </summary>
    /// <param name="requestedDays"></param>
    public void ReturnDays(int requestedDays)
    {
        UsedDays -= requestedDays;
        RemainingDays += requestedDays;
    }

    #endregion
}
