using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeContractTest
{
    [Fact]
    public void EmployeeAddContract_ThrowsInvalidOperationException_WhenExpiredAtIsLessThanStartedAt()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        ContractType contractType = ContractType.Employment;
        string contractDetails = "Details for employee contract";
        DateOnly startedAt = new(2025, 12, 12);
        DateOnly invalidExpiredAt = new(2020, 1, 1);

        var expectedExceptionMessage = "Contract expiration date must be greater than start date";

        // Act
        Action result = () => employee.AddContract(contractType, contractDetails, startedAt, invalidExpiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidContractDateData))]
    public void Create_ForContractDatePeriod_ThrowsInvalidOperationException_WhenAntoherContractsInCurrentDateRangeExist(DateOnly startedAt,
                                                                                                                         DateOnly? expiredAt,
                                                                                                                         string expectedExceptionMessage)
    {
        // Arrange
        ContractType contractType = ContractType.B2B;
        string contractDetails = "Details for contract";

        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 11, 1), new DateOnly(2024, 11, 11)),
            (new DateOnly(2024, 11, 20), new DateOnly(2024, 12, 15)),
            (new DateOnly(2025, 1, 20), new DateOnly(2025, 2, 1)),
            (new DateOnly(2025, 2, 5), new DateOnly(2025, 2, 10))
        );

        // Act
        Action result = () => employee.AddContract(contractType, contractDetails, startedAt, expiredAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetValidDateData))]
    public void Create_ForDateParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                      DateOnly? expiredAt,
                                                                      int? expectedTotalDuration)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        // Act
        employee.AddContract(ContractType.B2B, "Contract details", startedAt, expiredAt);

        EmployeeContract employeeContract = employee.EmployeeContracts.First();

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Theory]
    [MemberData(nameof(GetValidDateData))]
    public void Update_ForDateParams_SetsAppropriateTotalDuratonValue(DateOnly startedAt,
                                                                      DateOnly? expiredAt,
                                                                      int? expectedTotalDuration)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 1, 1), new DateOnly(2024, 2, 1))
        );

        EmployeeContract employeeContract = employee.EmployeeContracts.First();

        // Act
        employeeContract.Update(ContractType.B2B, "Contract details", startedAt, expiredAt);

        // Assert
        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Fact]
    public void Terminate_ThrowsInvalidOperationException_WhenTerminationDateIsFewerThanTodaysDate()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 1, 1), new DateOnly(2026, 1, 1))
        );

        DateOnly terminationDate = new(2025, 6, 5);
        DateTime todaysDate = new(2025, 6, 6);

        EmployeeContract employeeContract = employee.EmployeeContracts.First();

        var expectedExceptionMessage = "Contract termination date must be greater than today's date";

        // Act
        Action result = () => employeeContract.Terminate(todaysDate, terminationDate);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Terminate_ThrowsInvalidOperationException_WhenExpirationDateIsFewerThanTerminationDate()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 1, 1), new DateOnly(2026, 1, 1))
        );

        DateTime todaysDate = new(2025, 6, 2);
        DateOnly terminationDate = new(2025, 6, 2);
        DateOnly expirationDate = new(2025, 6, 1);

        EmployeeContract employeeContract = employee.EmployeeContracts.First();

        var expectedExceptionMessage = "Contract termination date must be greater than start date";

        // Act
        Action result = () => employeeContract.Terminate(todaysDate, terminationDate, expirationDate);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetValidDateDataForTermination))]
    public void Terminate_ForDateParam_SersAppropriateTotalDuratonAndExpiredAtValues(DateOnly terminationDate,
                                                                                     DateOnly? expirationDate,
                                                                                     int expectedTotalDuration)
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 1, 1), new DateOnly(2026, 1, 1))
        );

        DateTime todaysDate = new(2025, 6, 2);
        EmployeeContract employeeContract = employee.EmployeeContracts.First();

        // Act
        employeeContract.Terminate(todaysDate, terminationDate, expirationDate);

        // Assert
        employeeContract
            .TerminatedAt
            .Should()
            .Be(terminationDate);

        employeeContract
            .ExpiredAt
            .Should()
            .Be(expirationDate);

        employeeContract
            .TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }

    [Fact]
    public async Task AddDocument_AddsEmployeeDocumentInstanceToEmployeeDocumentList()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithContractList(
            (new DateOnly(2024, 1, 1), new DateOnly(2026, 1, 1))
        );
        
        Mock<IEmployeeDocumentRuleSet> employeeDocumentRuleSetMock = EmployeeDocumentHelper.CreateEmployeeDocumentRuleSetMock();
        EmployeeDocumentHelper.SetupIsDocumentNumberUniqueAsyncToReturnValue(employeeDocumentRuleSetMock, isRuleFailed: false);

        EmployeeContract employeeContract = employee.EmployeeContracts.First();
        EmployeeDocument employeeDocument = await EmployeeDocumentHelper.CreateEmployeeDocumentAsync(employeeDocumentRuleSetMock.Object);

        // Act
        employeeContract.AddDocument(employeeDocument);

        // Assert
        employeeContract
            .EmployeeDocuments
            .Should()
            .Contain(employeeDocument);
    }

    public static IEnumerable<object[]> GetValidDateData()
        => [
            [new DateOnly(2023, 6, 12), null, null],
            [new DateOnly(2023, 6, 12), new DateOnly(2024, 6, 12), 366],
            [new DateOnly(2023, 5, 11), new DateOnly(2025, 5, 11), 731],
            [new DateOnly(2024, 11, 20), new DateOnly(2028, 6, 3), 1291],
        ];

    public static IEnumerable<object[]> GetValidDateDataForTermination()
        => [
            [new DateOnly(2025, 6, 12), null, 528],
            [new DateOnly(2025, 6, 12), new DateOnly(2025, 7, 12), 558],
            [new DateOnly(2025, 5, 11), new DateOnly(2025, 5, 11), 496],
            [new DateOnly(2026, 1, 1), null, 731],
            [new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 1), 731],
        ];

    public static IEnumerable<object[]> GetInvalidContractDateData()
        => [
            // cases violating rule: "during the current contract"
            [new DateOnly(2024, 11, 05), new DateOnly(2024, 11, 10), "Cannot define another contract during the current contract"],
            [new DateOnly(2024, 11, 25), new DateOnly(2024, 12, 10), "Cannot define another contract during the current contract"],
            [new DateOnly(2025, 01, 22), new DateOnly(2025, 01, 30), "Cannot define another contract during the current contract"],
            [new DateOnly(2025, 02, 06), new DateOnly(2025, 02, 09), "Cannot define another contract during the current contract"],

            // cases violating rule: "indefinite-term during a contract"
            [new DateOnly(2024, 11, 05), null, "Cannot define indefinite-term contract during the current contract"],
            [new DateOnly(2025, 01, 21), null, "Cannot define indefinite-term contract during the current contract"],

            // cases violating rule: "during the indefinite-term contract"
            [new DateOnly(2024, 10, 15), new DateOnly(2024, 11, 01), "Cannot define another contract during the indefinite-term contract"],
            [new DateOnly(2024, 10, 15), null, "Cannot define another contract during the indefinite-term contract"],
            [new DateOnly(2024, 12, 01), new DateOnly(2025, 01, 01), "Cannot define another contract during the indefinite-term contract"],

            // extreme case: very late date but still a perpetual contract
            [new DateOnly(2030, 01, 01), new DateOnly(2031, 01, 01), "Cannot define another contract during the indefinite-term contract"],
        ];
}
