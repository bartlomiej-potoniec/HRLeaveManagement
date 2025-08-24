using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class TimeRegisterTest
{
    [Fact]
    public void Create_ForGivenWorkDateRange_ThrowsInvalidOperationException_WhenAnotherTimeRegisterWithGivenDateAlreadyExist()
    {
        // Arrange
        DateOnly existingRegisterDate = new(2024, 1, 20);

        Employee employeeWithTimeRegister = EmployeeHelper.CreateEmployeeWithTimeRegisterList(
            (existingRegisterDate, new TimeOnly(7, 0, 0), new TimeOnly(15, 0, 0), null, null)
        );

        var expectedExceptionMessage = "Time register for requested date already exists";

        // Act
        Action result = () => TimeRegister.Create(
            employeeWithTimeRegister,
            existingRegisterDate,
            new TimeOnly(8, 0, 0),
            new TimeOnly(16, 0, 0)
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenWorkTimeRange_ThrowsArgumentException_WhenWorkStartedAtIsGreaterThanWorkEndedAt()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        DateOnly registerDate = new(2024, 1, 20);
        TimeOnly workStartedAt = new(7, 0, 0);
        TimeOnly workEndedAt = new(15, 0, 0);

        var expectedExceptionMessage = "Start time of workday must be fewer than end time";

        // Act
        Action result = () => TimeRegister.Create(employee, registerDate, workStartedAt, workEndedAt);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [InlineData("08:15", null)]
    [InlineData(null, "08:45")]
    public void Create_ForGivenBreakTimeRange_ThrowsInvalidOperationException_WhenOnlyOneOfBreakTimeRangeHasValue(string? breakStartedAtText,
                                                                                                                  string? breakEndedAtText)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        TimeOnly? breakStartedAt = ConvertTimeTextToTimeOnly(breakStartedAtText);
        TimeOnly? breakEndedAt = ConvertTimeTextToTimeOnly(breakEndedAtText);

        var expectedExceptionMessage = "Work break must be a complete time range";

        // Act
        Action result = () => TimeRegister.Create(
            employee,
            new DateOnly(2024, 1, 20),
            new TimeOnly(7, 0, 0),
            new TimeOnly(15, 0, 0),
            breakStartedAt,
            breakEndedAt
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [InlineData("06:59", "8:00")]
    [InlineData("8:00", "15:45")]
    [InlineData("7:00", "15:00")]
    [InlineData("6:00", "16:00")]
    public void Create_ForGivenBreakTimeRange_ThrowsInvalidOperationException_WhenBreakTimeRangeIsNotIncludedInWorkTime(string? breakStartedAtText,
                                                                                                                        string? breakEndedAtText)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        TimeOnly workStartedAt = new(7, 0, 0);
        TimeOnly workEndedAt = new(15, 0, 0);
        TimeOnly? breakStartedAt = ConvertTimeTextToTimeOnly(breakStartedAtText);
        TimeOnly? breakEndedAt = ConvertTimeTextToTimeOnly(breakEndedAtText);

        var expectedExceptionMessage = "Work break time must be included in work time";

        // Act
        Action result = () => TimeRegister.Create(
            employee,
            new DateOnly(2024, 1, 20),
            workStartedAt,
            workEndedAt,
            breakStartedAt,
            breakEndedAt
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetBreakTimeData))]
    public void Create_ForGivenBreakTimeRange_SetsAppropriateTotalBreakTimeValues(TimeOnly? breakStartedAt,
                                                                                  TimeOnly? breakEndedAt,
                                                                                  TimeSpan? expectedTotalBreakTime)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        // Act 
        TimeRegister timeRegister = TimeRegister.Create(
            employee,
            new DateOnly(2024, 1, 20),
            new TimeOnly(7, 0, 0),
            new TimeOnly(15, 0, 0),
            breakStartedAt,
            breakEndedAt
        );

        // Assert
        timeRegister.TotalBreakTime
            .Should()
            .Be(expectedTotalBreakTime);
    }

    [Theory]
    [MemberData(nameof(GetBreakTimeData))]
    public void Update_ForGivenBreakTimeRange_SetsAppropriateTotalBreakTimeValues(TimeOnly? breakStartedAtToUpdate,
                                                                                  TimeOnly? breakEndedAtToUpdate,
                                                                                  TimeSpan? expectedTotalBreakTime)
    {
        // Arrange
        DateOnly existingRegisterDate = new(2024, 1, 20);
        TimeOnly existingWorkStartedAt = new(7, 0, 0);
        TimeOnly existingWorkEndedAt = new(15, 0, 0);

        Employee employee = EmployeeHelper.CreateEmployeeWithTimeRegisterList(
            (existingRegisterDate, existingWorkStartedAt, existingWorkEndedAt, null, null)    
        );

        TimeRegister timeRegister = employee.TimeRegisters.First();

        // Act 
        timeRegister.Update(existingWorkStartedAt, existingWorkEndedAt, breakStartedAtToUpdate, breakEndedAtToUpdate);

        // Assert
        timeRegister.TotalBreakTime
            .Should()
            .Be(expectedTotalBreakTime);
    }

    public static IEnumerable<object[]?> GetBreakTimeData()
        => [
            [null, null, null],
            [new TimeOnly(7, 30), null, null],
            [null, new TimeOnly(15, 30), null],
            [new TimeOnly(7, 30), new TimeOnly(15, 30), new TimeSpan(8, 0, 0)],
            [new TimeOnly(12, 45), new TimeOnly(13, 0), new TimeSpan(0, 15, 0)]
        ];

    #region Test_Factory_Methods

    private static TimeOnly? ConvertTimeTextToTimeOnly(string? timeText) => timeText is null ? null : TimeOnly.Parse(timeText);

    #endregion
}
