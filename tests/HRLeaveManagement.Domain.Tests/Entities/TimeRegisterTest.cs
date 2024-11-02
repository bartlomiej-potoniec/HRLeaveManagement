namespace HRLeaveManagement.Domain.Tests.Entities;

public class TimeRegisterTest
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void Create_ForGivenBreakTimeParams_SetsAppropriateTotalBreakTimeValue(TimeOnly? breakStartedAt,
                                                                                  TimeOnly? breakEndedAt,
                                                                                  TimeSpan? expectedTotalBreakTime)
    {
        // Act 
        var timeRegister = CreateForEmployeeBreak(breakStartedAt, breakEndedAt);

        // Assert
        timeRegister
            .TotalBreakTime
            .Should()
            .Be(expectedTotalBreakTime);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Update_ForGivenBreakTimeParams_SetsAppropriateTotalBreakTimeValue(TimeOnly? breakStartedAt,
                                                                                  TimeOnly? breakEndedAt,
                                                                                  TimeSpan? expectedTotalBreakTime)
    {
        // Arrange
        var timeRegister = CreateForEmployeeBreak();

        //Act
        TimeRegister.Update(
            timeRegister,
            timeRegister.EmployeeId,
            timeRegister.RegisterDate,
            timeRegister.WorkStartedAt,
            timeRegister.WorkEndedAt,
            breakStartedAt,
            breakEndedAt
        );

        // Assert
        timeRegister
            .TotalBreakTime
            .Should()
            .Be(expectedTotalBreakTime);
    }

    public static IEnumerable<object[]?> GetData()
        => [
            [null, null, null],
            [new TimeOnly(7, 30), null, null],
            [null, new TimeOnly(15, 30), null],
            [new TimeOnly(7, 30), new TimeOnly(15, 30), new TimeSpan(8, 0, 0)],
            [new TimeOnly(12, 45), new TimeOnly(13, 0), new TimeSpan(0, 15, 0)]
        ];


    private static TimeRegister CreateForEmployeeBreak(TimeOnly? breakStartedAt = null,
                                                       TimeOnly? brakeEndedAt = null)
        =>
            TimeRegister.Create(
                employeeId: Guid.NewGuid(),
                registerDate: new DateOnly(2024, 6, 20),
                workStartedAt: new TimeOnly(8, 30),
                workEndedAt: new TimeOnly(15, 30),
                breakStartedAt,
                brakeEndedAt
            );
}
