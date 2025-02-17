using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeExperienceTest
{
    [Fact]
    public void Create_ForGivenEmploymentParams_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateOnly employedFrom = new(2023, 06, 20);
        DateOnly employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmploymentDates(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Create_ForGivenEmploymentDateTimes_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        DateTime employedFrom = new(2023, 06, 20);
        DateTime employedTo = new(2024, 06, 20);
        int expectedTotalEmployment = 366;

        // Act
        var employeeExperience = CreateForEmploymentDateTimes(employedFrom, employedTo);

        // Assert
        employeeExperience
            .TotalEmployment
            .Should()
            .Be(expectedTotalEmployment);
    }

    [Fact]
    public void Update_ForGivenEmploymentParams_SetsAppropriateTotalEmploymentValue()
    {
        // Arrange
        var employeeExperience = CreateForEmploymentDates();

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

    private EmployeeExperience CreateForEmploymentDates(DateOnly employedFrom = new(),
                                                        DateOnly employedTo = new())
        => EmployeeExperience.Create(Guid.NewGuid(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);

    private EmployeeExperience CreateForEmploymentDateTimes(DateTime employedFrom = new(),
                                                            DateTime employedTo = new())
        => EmployeeExperience.Create(EmployeeHelper.CreateEmployee(), ContractType.B2B, "Company", "Logistic", employedFrom, employedTo);
}
