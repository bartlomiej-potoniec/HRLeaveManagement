using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeExperienceTest
{
    [Fact]
    public void Create_ThrowsArgumentException_WhenEmployeeIsNull()
    {
        // Arrange
        Employee employee = null;
        var expectedExceptionMessage = "Employee must be included";

        // Act
        Action result = () => EmployeeExperience.Create(employee, ContractType.B2B, "Company", "Engineer", new DateOnly(), new DateOnly());

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidData))]
    public void Create_ThrowsArgumentException_WhenPreviousCompanyNameIsNullOrEmpty(string previousCompanyName)
    {
        // Arrange
        var expectedExceptionMessage = "Previous company's name of employee cannot be empty";

        // Act
        Action result = () => CreateWithExperienceDates(previousCompanyName: previousCompanyName);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidData))]
    public void Create_ThrowsArgumentException_WhenPositionIsNullOrEmpty(string position)
    {
        // Arrange
        var expectedExceptionMessage = "Position at previous company of employee cannot be empty";

        // Act
        Action result = () => CreateWithExperienceDates(position: position);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ThrowsInvalidOperationException_WhenEmployedToIsLessThanEmployedFrom()
    {
        // Arrange
        DateOnly employedFrom = new(2026, 1, 1);
        DateOnly employedTo = new(2024, 1, 1);
        var expectedExceptionMessage = "Employment end date at previous company must be greater than start date";

        // Act
        Action result = () => CreateWithExperienceDates(employedFrom: employedFrom, employedTo: employedTo);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Update_ForGivenDates_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        var employeeExperience = CreateWithExperienceDates();

        DateOnly employedFrom = new(2023, 05, 20);
        DateOnly employedTo = new(2024, 06, 20);
        string previousCompanyName = "Company2";
        string position = "R&D";
        int expectedTotalEmployment = 397;

        // Act
        EmployeeExperience.Update(
            employeeExperience,
            employeeExperience.ContractType,
            previousCompanyName,
            position,
            employedFrom,
            employedTo
        );

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Update_ForGivenDateTimes_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        var employeeExperience = CreateWithExperienceDateTimes();

        DateTime employedFrom = new(2023, 05, 20);
        DateTime employedTo = new(2024, 06, 20);
        string previousCompanyName = "Company2";
        string position = "R&D";
        int expectedTotalEmployment = 397;

        // Act
        EmployeeExperience.Update(
            employeeExperience,
            employeeExperience.ContractType,
            previousCompanyName,
            position,
            employedFrom,
            employedTo
        );

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    public static IEnumerable<object[]?> GetInvalidData() => [[null], [""]];

    private static EmployeeExperience CreateWithExperienceDates(ContractType contractType = ContractType.B2B,
                                                                string previousCompanyName = "Company",
                                                                string position = "Engineer",
                                                                DateOnly employedFrom = new(),
                                                                DateOnly employedTo = new())
        => EmployeeExperience.Create(EmployeeHelper.CreateEmployee(), contractType, previousCompanyName, position, employedFrom, employedTo);

    private static EmployeeExperience CreateWithExperienceDateTimes(ContractType contractType = ContractType.B2B,
                                                                    string previousCompanyName = "Company",
                                                                    string position = "Engineer",
                                                                    DateTime employedFrom = new(),
                                                                    DateTime employedTo = new())
        => EmployeeExperience.Create(EmployeeHelper.CreateEmployee(), contractType, previousCompanyName, position, employedFrom, employedTo);
}
