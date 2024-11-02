namespace HRLeaveManagement.Domain.Tests.Entities;

public class RemoteWorkLimitTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        var remoteWorkLimit = CreateWithDefaultValues();

        // Assert
        remoteWorkLimit
            .Should()
            .BeOfType<RemoteWorkLimit>();
    }

    [Fact]
    public void Update_ForGivenAvailableDaysParam_SetsAppropriateRemainingDaysValue()
    {
        // Arrange
        var remoteWorkLimit = CreateWithDefaultValues();

        int requestedAvailableDays = 20;
        int expectedRemainingDays = 20;

        // Act
        RemoteWorkLimit.Update(
            remoteWorkLimit,
            remoteWorkLimit.EmployeeId,
            remoteWorkLimit.Year,
            requestedAvailableDays
        );

        // Assert
        remoteWorkLimit
            .RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }

    [Fact]
    public void UpdateDays_ForGivenAvailableDaysParam_SetsAppropriateRemainingDaysValue()
    {
        // Arrange
        var remoteWorkLimit = CreateWithDefaultValues();

        int requestedDays = 8;
        int expectedRemainingDays = 18;

        // Act
        RemoteWorkLimit.UpdateDays(remoteWorkLimit, requestedDays);

        // Assert
        remoteWorkLimit
            .RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }


    private static RemoteWorkLimit CreateWithDefaultValues()
        => RemoteWorkLimit.Create(Guid.NewGuid(), 2024, 26);
}
