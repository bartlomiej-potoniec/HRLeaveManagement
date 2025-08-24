using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeExperienceTest
{
    [Fact]
    public void EmployeeAddExperience_ThrowsInvalidOperationException_WhenEmployedToIsLessThanEmployedFrom()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        DateOnly employedFrom = new(2026, 1, 1);
        DateOnly employedTo = new(2024, 1, 1);

        var expectedExceptionMessage = "Employment end date at previous company must be greater than start date";

        // Act
        Action result = () => employee.AddExperience(
            ContractType.Employment,
            "Previous company sp. z.o.o.",
            "Engineer",
            employedFrom,
            employedTo,
            "Experience details"
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void EmployeeAddExperience_ForGivenEmploymentDateRange_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        ContractType contractType = ContractType.Employment;
        string previousCompanyName = "Previous company sp. z.o.o.";
        string position = "Engineer";
        DateOnly employedFrom = new(2023, 1, 1);
        DateOnly employedTo = new(2024, 1, 1);
        string experienceDetails = "Experience details";

        int expectedTotalEmployment = 365;

        // Act
        employee.AddExperience(
            contractType,
            previousCompanyName,
            position,
            employedFrom,
            employedTo,
            experienceDetails
        );

        EmployeeExperience employeeExperience = employee.EmployeeExperiences.First();

        // Assert
        employeeExperience.TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact] 
    public void Update_ForGivenDates_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithExperienceList(
            (new DateOnly(2023, 1, 20), new DateOnly(2024, 5, 15))
        );

        DateOnly employedFrom = new(2023, 5, 20);
        DateOnly employedTo = new(2024, 6, 20);
        int expectedTotalEmployment = 397;

        EmployeeExperience employeeExperience = employee.EmployeeExperiences.First();

        // Act
        employeeExperience.Update(
            employeeExperience.ContractType,
            employeeExperience.PreviousCompanyName,
            employeeExperience.Position,
            employedFrom,
            employedTo
        );

        // Assert
        employeeExperience.EmployedFrom
            .Should()
            .Be(employedFrom);

        employeeExperience.EmployedTo
            .Should()
            .Be(employedTo);

        employeeExperience.TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public async Task AddDocument_AddsEmployeeDocumentInstanceToEmployeeDocumentList()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithExperienceList(
            (new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1))
        );

        Mock<IEmployeeDocumentRuleSet> employeeDocumentRuleSetMock = EmployeeDocumentHelper.CreateEmployeeDocumentRuleSetMock();
        EmployeeDocumentHelper.SetupIsDocumentNumberUniqueAsyncToReturnValue(employeeDocumentRuleSetMock, isRuleFailed: false);

        EmployeeExperience employeeExperience = employee.EmployeeExperiences.First();
        EmployeeDocument employeeDocument = await EmployeeDocumentHelper.CreateEmployeeDocumentAsync(employeeDocumentRuleSetMock.Object);

        // Act
        employeeExperience.AddDocument(employeeDocument);

        // Assert
        employeeExperience
            .EmployeeDocuments
            .Should()
            .Contain(employeeDocument);
    }
}
