using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeEducationTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        var employeeEducation = CreateWithDefaultValues();

        // Assert
        employeeEducation
            .Should()
            .BeOfType<EmployeeEducation>();
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        Guid emloyeeId = Guid.NewGuid();
        EducationType educationType = EducationType.Higher;
        string educationDetails = "Details";
        DateOnly enrolledAt = new();
        DateOnly graduatedAt = new();

        var employeeEducation = CreateWithDefaultValues();
        var expectedEmployeeEducation = EmployeeEducation.Create(
            emloyeeId,
            educationType,
            educationDetails,
            enrolledAt,
            graduatedAt
        );

        // Act
        EmployeeEducation.Update(
            employeeEducation,
            emloyeeId,
            educationType,
            educationDetails,
            enrolledAt,
            graduatedAt
        );

        // Assert
        employeeEducation
            .Should()
            .BeEquivalentTo(expectedEmployeeEducation, options => options
                .Including(ee => ee.EmployeeId)
                .Including(ee => ee.EducationType)
                .Including(ee => ee.EducationDetails)
                .Including(ee => ee.EnrolledAt)
                .Including(ee => ee.GraduatedAt)
            );
    }


    private static EmployeeEducation CreateWithDefaultValues()
        =>
            EmployeeEducation.Create(
                employeeId: Guid.NewGuid(),
                educationType: EducationType.Secondary,
                educationDetails: string.Empty,
                enrolledAt: new DateOnly(),
                graduatedAt: new DateOnly()
            );
}
