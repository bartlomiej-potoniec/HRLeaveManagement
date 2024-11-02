namespace HRLeaveManagement.Domain.Tests.Entities;

public class LeaveAllocationTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewinstance()
    {
        // Act
        var leaveAllocation = CreateWithAvailableDays(20);

        // Assert
        leaveAllocation
            .Should()
            .BeOfType<LeaveAllocation>();
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData(20, null, 0, 20)]
    [InlineData(null, 5, null, null)]
    [InlineData(20, 5, 5, 15)]
    [InlineData(26, 7, 7, 19)]
    public void UseDays_ForGivenRequestedDays_SetsAppropriateUsedAndRemaningDaysValue(int? availableDays,
                                                                                      int? requestedDays,
                                                                                      int? expectedUsedDays,
                                                                                      int? expectedRemainingDays)
    {
        // Arrange
        var leaveAllocation = CreateWithAvailableDays(availableDays);

        // Act
        LeaveAllocation.UseDays(leaveAllocation, requestedDays);

        // Assert
        leaveAllocation
            .UsedDays
            .Should()
            .Be(expectedUsedDays);

        leaveAllocation
            .RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }

    [Theory]
    [InlineData(20, 5, 5, 10, 10)]
    [InlineData(26, 2, 12, 14, 12)]
    [InlineData(26, 15, 7, 22, 4)]
    public void UseDays_ForGivenRequestedDays_IncrementsUsedAndRemainingDaysValue(int availableDays,
                                                                                  int alreadyUsedDays,
                                                                                  int requestedDays,
                                                                                  int expectedUsedDays,
                                                                                  int expectedRemainingDays)
    {
        // Arrange
        var leaveAllocation = CreateWithAvailableDays(availableDays);
        LeaveAllocation.UseDays(leaveAllocation, alreadyUsedDays);

        // Act
        LeaveAllocation.UseDays(leaveAllocation, requestedDays);

        // Assert
        leaveAllocation
            .UsedDays
            .Should()
            .Be(expectedUsedDays);

        leaveAllocation
            .RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }

    [Theory]
    [InlineData(null, null, null, null, null)]
    [InlineData(null, null, 20, 0, 20)]
    [InlineData(20, 0, null, null, null)]
    [InlineData(20, 0, 26, 0, 26)]
    [InlineData(20, 10, 26, 10, 16)]
    [InlineData(26, 7, 20, 7, 13)]
    public void Update_ForGivenAvailableDaysParams_SetsAppropriateUsedAndRemaningDaysValue(int? alreadyAvailableDays,
                                                                                           int? alreadyUsedDays,
                                                                                           int? requestedAvailableDays,
                                                                                           int? expectedUsedDays,
                                                                                           int? expectedRemainingDays)
    {
        // Arrange
        var leaveAllocation = CreateWithAvailableDays(alreadyAvailableDays);
        LeaveAllocation.UseDays(leaveAllocation, alreadyUsedDays);

        // Act
        UpdateWithAvailableDays(leaveAllocation, requestedAvailableDays);

        // Assert
        leaveAllocation
            .UsedDays
            .Should()
            .Be(expectedUsedDays);

        leaveAllocation
            .RemainingDays
            .Should()
            .Be(expectedRemainingDays);
    }


    private static LeaveAllocation CreateWithAvailableDays(int? availableDays = null)
        => LeaveAllocation.Create(Guid.NewGuid(), 1, 2024, availableDays);

    private static void UpdateWithAvailableDays(LeaveAllocation entity, int? availableDays = null)
        => LeaveAllocation.Update(entity, Guid.NewGuid(), 2, 2024, availableDays);
}
