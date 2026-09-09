using HRLeaveManagement.Domain.Tests.Helpers;
using HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class RemoteWorkLimitTest
{
    [Fact]
    public void Create_ForGivenYear_ThrowsInvalidOperationException_WhenRemoteWorkLimitForGivenYearAlreadyExist()
    {
        // Arrange
        int year = 2024;
        int currentYear = year;
        int availableDays = 150;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, availableDays));

        var expectedExceptionMessage = "Remote work limit for requested year already exists";

        // Act
        Action result = () => RemoteWorkLimit.Create(employee, currentYear, year, availableDays);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenYear_ThrowsArgumentException_WhenGivenYearIsFewerThanCurrentYear()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        int year = 2023;
        int currentYear = 2024;
        int availableDays = 200;

        var expectedExceptionMessage = "Changes on remote work limits may only be performed for current and future year";

        // Act
        Action result = () => RemoteWorkLimit.Create(employee, currentYear, year, availableDays);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenAvailableDays_ThrowsArgumentException_WhenGivenAvailableDaysIsFewerThanZero()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        int year = 2024;
        int currentYear = year;
        int invalidAvailableDays = -20;

        var expectedExceptionMessage = "Available days in remote work limit must be a positive number";

        // Act
        Action result = () => RemoteWorkLimit.Create(employee, currentYear, year, invalidAvailableDays);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenAvailableDays_ThrowsArgumentException_WhenGivenAvailableDaysIsGreaterThanDaysInGivenYear()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        int leapYear = 2024;
        int currentYear = leapYear;
        int invalidAvailableDays = 367;

        var expectedExceptionMessage = "Available days in remote work limit must be fewer than days in requested year";

        // Act
        Action result = () => RemoteWorkLimit.Create(employee, currentYear, leapYear, invalidAvailableDays);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        int year = 2024;
        int currentYear = year;
        int availableDays = 200;

        var expectedRemoteWorkLimit = new
        {
            Employee = employee,
            Year = year,
            AvailableDays = availableDays,
            UsedDays = 0,
            RemainingDays = availableDays,
        };

        // Act
        RemoteWorkLimit remoteWorkLimit = RemoteWorkLimit.Create(employee, currentYear, year, availableDays);

        // Assert
        remoteWorkLimit
            .Should()
            .BeEquivalentTo(expectedRemoteWorkLimit, options => options
                .Including(limit => limit.Employee)
                .Including(limit => limit.Year)
                .Including(limit => limit.AvailableDays)
                .Including(limit => limit.UsedDays)
                .Including(limit => limit.RemainingDays)
            );
    }

    [Fact]
    public void Update_ForGivenAvailableDaysParam_ThrowsInvalidOperationException_WhenGivenAvailableDaysIsFewerThanCurrentlyUsedDays()
    {
        // Arrange
        int year = 2024;
        int currentYear = year;
        int currentlyAvailableDays = 100;
        int currentlyUsedDays = 40;
        int invalidAvailableDays = 30;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, currentlyAvailableDays));
        
        RemoteWorkLimit remoteWorkLimit = employee.RemoteWorkLimits.First();
        remoteWorkLimit.UseDays(currentlyUsedDays);

        var expectedExceptionMessage = "Given available days value may not be fewer than currently used days";

        // Act
        Action result = () => remoteWorkLimit.Update(currentYear, year, invalidAvailableDays);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Update_ForGivenAvailableDaysParam_SetsAppropriateRemainingDaysValue()
    {
        // Arrange
        int year = 2024;
        int currentYear = year;
        int currentlyAvailableDays = 100;
        int currentlyUsedDays = 40;

        int requestedAvailableDays = 50;
        int expectedRemainingDays = 10;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, currentlyAvailableDays));

        RemoteWorkLimit remoteWorkLimit = employee.RemoteWorkLimits.First();
        remoteWorkLimit.UseDays(currentlyUsedDays);

        // Act
        remoteWorkLimit.Update(currentYear, year, requestedAvailableDays);

        // Assert
        remoteWorkLimit.AvailableDays
            .Should()
            .Be(requestedAvailableDays);

        remoteWorkLimit.UsedDays
            .Should()
            .Be(currentlyUsedDays);

        remoteWorkLimit.RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }

    [Fact]
    public void UseDays_ForGivenRequestedDays_ThrowsArgumentException_WhenGivenRequestedDaysIsFewerThanZero()
    {
        // Arrange
        int year = 2024;
        int availableDays = 100;
        int invalidRequestedDays = -2;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, availableDays));
        RemoteWorkLimit remoteWorkLimit = employee.RemoteWorkLimits.First();

        var expectedExceptionMessage = "Requested days in remote work limit must be a positive number";

        // Act
        Action result = () => remoteWorkLimit.UseDays(invalidRequestedDays);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void UseDays_ForGivenRequestedDays_ThrowsArgumentException_WhenGivenRequestedDaysIsGreaterThanRemainingDays()
    {
        // Arrange
        int year = 2024;
        int currentlyAvailableDays = 80;
        int currentlyUsedDays = 40;
        int invalidRequestedDays = 50;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, currentlyAvailableDays));
        
        RemoteWorkLimit remoteWorkLimit = employee.RemoteWorkLimits.First();
        remoteWorkLimit.UseDays(currentlyUsedDays);

        var expectedExceptionMessage = "Requested days in remote work limit must be fewer than available days";

        // Act
        Action result = () => remoteWorkLimit.UseDays(invalidRequestedDays);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void UseDays_ForGivenAvailableDaysParam_SetsAppropriateRemainingDaysValue()
    {
        // Arrange
        int year = 2024;
        int availableDays = 100;
        int requestedDays = 8;
        int expectedRemainingDays = 92;

        Employee employee = EmployeeHelper.CreateEmployeeWithRemoteWorkList((year, availableDays));
        RemoteWorkLimit remoteWorkLimit = employee.RemoteWorkLimits.First();

        // Act
        remoteWorkLimit.UseDays(requestedDays);

        // Assert
        remoteWorkLimit.UsedDays
            .Should()
            .Be(requestedDays);

        remoteWorkLimit.RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }
}
