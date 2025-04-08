using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeExperienceTest
{
    [Fact]
    public void Create_ForGivenEmployeeAndDatesOnly_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateOnly employedFrom = new(2023, 06, 20);
        DateOnly employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmployee(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Create_ForGivenEmployeeAndDateTimes_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateTime employedFrom = new(2023, 06, 20);
        DateTime employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmployee(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Create_ForGivenEmployeeIdAndDatesOnly_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateOnly employedFrom = new(2023, 06, 20);
        DateOnly employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmployeeId(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }


    [Fact]
    public void Create_ForGivenEmployeeIdAndDateTimes_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateTime employedFrom = new(2023, 06, 20);
        DateTime employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmployeeId(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Update_ForGivenEmployeeExperienceAndDatesOnly_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        var employeeExperience = CreateForEmployee(new DateOnly(2023, 05, 20), new DateOnly(2024, 06, 20));

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
    public void Update_ForGivenEmployeeExperienceAndDateTimes_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        var employeeExperience = CreateForEmployee(new DateTime(2023, 05, 20), new DateTime(2024, 06, 20));

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

    private static EmployeeExperience CreateForEmployeeId(DateOnly employedFrom = new(),
                                                   DateOnly employedTo = new())
        => EmployeeExperience.Create(Guid.NewGuid(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);

    private static EmployeeExperience CreateForEmployeeId(DateTime employedFrom = new(),
                                                   DateTime employedTo = new())
        => EmployeeExperience.Create(Guid.NewGuid(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);

    private static EmployeeExperience CreateForEmployee(DateOnly employedFrom = new(),
                                                  DateOnly employedTo = new())
        => EmployeeExperience.Create(EmployeeHelper.CreateEmployee(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);

    private static EmployeeExperience CreateForEmployee(DateTime employedFrom = new(),
                                                  DateTime employedTo = new())
        => EmployeeExperience.Create(EmployeeHelper.CreateEmployee(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);
}
