using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeContractTest
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void Create_ForGivenEmploymentParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                                 DateOnly? expiredAt,
                                                                                 int? expectedTotalDuration)
    {
        // Act
        var employeeContract = CreateWithEmploymentDates(startedAt, expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Update_ForGivenEmploymentParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                                 DateOnly? expiredAt,
                                                                                 int? expectedTotalDuration)
    {
        // Arrange
        var employeeContract = CreateWithEmploymentDates();

        // Act
        EmployeeContract.Update(
            employeeContract,
            employeeContract.EmployeeId,
            employeeContract.ContractType,
            startedAt,
            expiredAt
        );

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Fact]
    public void Terminate_ForGivenExpiredDate_SetsAppropriateTotalDuratonValue()
    {
        // Arrange
        DateOnly startedAt = new(2023, 06, 20);
        DateOnly expiredDate = new(2024, 06, 20);
        int expectedTotalDuration = 366;

        var employeeContract = CreateWithEmploymentDates(startedAt);

        // Act
        EmployeeContract.Terminate(employeeContract, expiredDate);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    public static IEnumerable<object[]?> GetData()
        => [
            [new DateOnly(2023, 6, 12), null, null],
            [new DateOnly(2023, 6, 12), new DateOnly(2024, 6, 12), 366]
        ];


    private static EmployeeContract CreateWithEmploymentDates(DateOnly startedAt = new(),
                                                              DateOnly? expiredAt = null)
        =>
            EmployeeContract.Create(Guid.NewGuid(), ContractType.Employment, startedAt, expiredAt);
}
