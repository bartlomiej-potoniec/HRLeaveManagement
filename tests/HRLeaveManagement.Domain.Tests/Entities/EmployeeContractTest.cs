using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Data;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeContractTest
{
    [Fact]
    public void Create_ThrowsArgumentException_WhenEmployeeIsNull()
    {
        // Arrange
        Employee invalidEmployee = null;
        var expectedExceptionMessage = "Employee must be included";

        // Act
        Action result = () => EmployeeContract.Create(invalidEmployee, ContractType.B2B, new DateOnly(), null);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ThrowsInvalidOperationException_WhenExpiredAtIsLessThanStartedAt()
    {
        // Arrange
        DateOnly startedAt = new(2025, 12, 12);
        DateOnly invalidExpiredAt = new(2020, 1, 1);
        var expectedExceptionMessage = "Contract expiration date must be greater than start date";

        // Act
        Action result = () => CreateWithEmploymentDates(startedAt: startedAt, expiredAt: invalidExpiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [ClassData(typeof(InvalidContractWithDatesTestData))]
    public void Create_ForContractDatePeriod_ThrowsInvalidOperationException_WhenAntoherContractsInCurrentTermExist(DateOnly startedAt,
                                                                                                                    DateOnly? expiredAt,
                                                                                                                    IEnumerable<EmployeeContract> employeeContracts,
                                                                                                                    string expectedExceptionMessage)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee(contracts: employeeContracts);

        // Act
        Action result = () => EmployeeContract.Create(employee, ContractType.B2B, startedAt, expiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [ClassData(typeof(InvalidContractWithDateTimesTestData))]
    public void Create_ForContractDateTimePeriod_ThrowsInvalidOperationException_WhenAntoherContractsInCurrentTermExist(DateTime startedAt,
                                                                                                                        DateTime? expiredAt,
                                                                                                                        IEnumerable<EmployeeContract> employeeContracts,
                                                                                                                        string expectedExceptionMessage)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee(contracts: employeeContracts);

        // Act
        Action result = () => EmployeeContract.Create(employee, ContractType.B2B, startedAt, expiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetValidDataForDates))]
    public void Create_ForDateParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                      DateOnly? expiredAt,
                                                                      int? expectedTotalDuration)
    {
        // Act
        var employeeContract = CreateWithEmploymentDates(startedAt: startedAt, expiredAt: expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Theory]
    [MemberData(nameof(GetValidDataForDateTime))]
    public void Create_ForDateTimeParams_SetsAppropriateTotalDuratonAndExpiredAtValues(DateTime startedAt,
                                                                                       DateTime? expiredAt,
                                                                                       DateOnly? expectedExpiredAt,
                                                                                       int? expectedTotalDuration)
    {
        // Act
        var employeeContract = CreateWithEmploymentDateTimes(startedAt: startedAt, expiredAt: expiredAt);

        // Assert
        employeeContract
            .ExpiredAt
            .Should()
            .Be(expectedExpiredAt);

        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Theory]
    [MemberData(nameof(GetValidDataForDates))]
    public void Update_ForDateParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                      DateOnly? expiredAt,
                                                                      int? expectedTotalDuration)
    {
        // Arrange
        EmployeeContract employeeContract = CreateWithEmploymentDates();

        // Act
        EmployeeContract.Update(employeeContract, ContractType.B2B, startedAt, expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Theory]
    [MemberData(nameof(GetValidDataForDateTime))]
    public void Update_ForDateTimeParams_SetsAppropriateTotalDuratonAndExpiredAtValues(DateTime startedAt,
                                                                                       DateTime? expiredAt,
                                                                                       DateOnly? expectedExpiredAt,
                                                                                       int? expectedTotalDuration)
    {
        // Arrange
        EmployeeContract employeeContract = CreateWithEmploymentDates();

        // Act
        EmployeeContract.Update(employeeContract, ContractType.B2B, startedAt, expiredAt);

        // Assert
        employeeContract
            .ExpiredAt
            .Should()
            .Be(expectedExpiredAt);

        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Fact]
    public void Terminate_ThrowsInvalidOperationException_WhenExpiredDateIsLessThanStartedAt()
    {
        // Arrange
        DateOnly startedAt = new(2025, 6, 6);
        DateOnly invalidExpiredAt = new(2025, 1, 1);
        EmployeeContract employeeContract = CreateWithEmploymentDates(startedAt: startedAt);

        var expectedExceptionMessage = "Contract termination date must be greater than start date";

        // Act
        Action result = () => EmployeeContract.Terminate(employeeContract, invalidExpiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Terminate_ForDateParam_SersAppropriateTotalDuratonAndExpiredAtValues()
    {
        // Arrange
        DateOnly startedAt = new(2025, 6, 6);
        DateOnly expiredAt = new(2026, 6, 6);
        int expectedTotalDuration = 365;

        EmployeeContract employeeContract = CreateWithEmploymentDates(startedAt: startedAt);

        // Act
        EmployeeContract.Terminate(employeeContract, expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);

        employeeContract
            .ExpiredAt
            .Should()
            .Be(expiredAt);
    }

    [Fact]
    public void Terminate_ForDateTimeParam_SersAppropriateTotalDuratonAndExpiredAtValues()
    {
        // Arrange
        DateTime startedAt = new(2025, 6, 6);
        DateTime expiredAt = new(2026, 6, 6);
        DateOnly expectedExpiredAt = new(2026, 6, 6);
        int expectedTotalDuration = 365;
        
        EmployeeContract employeeContract = CreateWithEmploymentDateTimes(startedAt: startedAt);

        // Act
        EmployeeContract.Terminate(employeeContract, expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);

        employeeContract
            .ExpiredAt
            .Should()
            .Be(expectedExpiredAt);
    }

    public static IEnumerable<object[]?> GetValidDataForDates()
        => [
            [new DateOnly(2023, 6, 12), null, null],
            [new DateOnly(2023, 6, 12), new DateOnly(2024, 6, 12), 366]
        ];

    public static IEnumerable<object[]?> GetValidDataForDateTime()
        => [
            [new DateTime(2023, 6, 12), null, null, null],
            [new DateTime(2023, 6, 12), new DateTime(2024, 6, 12), new DateOnly(2024, 6, 12), 366]
        ];

    #region Test_Factory_Methods

    private static EmployeeContract CreateWithEmploymentDates(ContractType contractType = ContractType.Employment,
                                                              DateOnly startedAt = new(),
                                                              DateOnly? expiredAt = null)
        =>
            EmployeeContract.Create(EmployeeHelper.CreateEmployee(), contractType, startedAt, expiredAt);

    private static EmployeeContract CreateWithEmploymentDateTimes(ContractType contractType = ContractType.Employment,
                                                                  DateTime startedAt = new(),
                                                                  DateTime? expiredAt = null)
        =>
            EmployeeContract.Create(EmployeeHelper.CreateEmployee(), contractType, startedAt, expiredAt);

    #endregion
}
