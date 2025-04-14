using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeEducationTest
{
    [Fact]
    public void Create_ThrowsArgumentException_WhenEmployeeIsNull()
    {
        // Arrange
        Employee employee = null;
        var expectedExceptionMessage = "Employee must be included";

        // Act
        Action result = () => EmployeeEducation.Create(employee, EducationType.Basic, "Details", new DateOnly(), new DateOnly());

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(GetInvalidDataForEducationDetails))]
    public void Create_ThrowsArgumentException_WhenEducationDetailsIsNullOrEmpty(string educationDetails)
    {
        // Arrange
        var expectedExceptionMessage = "Education details for employee cannot be empty";

        // Act
        Action result = () => CreateWithEducationDates(educationDetails: educationDetails);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ThrowsInvalidOperationException_WhenGraduatedAtIsLessThanEnrolledAt()
    {
        // Arrange
        DateOnly enrolledAt = new(2025, 6, 6);
        DateOnly invalidGraduatedAt = new(2025, 1, 1);
        var expectedExceptionMessage = "Education graduation date must be greater than enroll date";

        // Act
        Action result = () => CreateWithEducationDates(enrolledAt: enrolledAt, graduatedAt: invalidGraduatedAt);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstanceOfEmployeeEducation()
    {
        // Act
        EmployeeEducation employeeEducation = CreateWithEducationDates();

        // Assert
        employeeEducation
            .Should()
            .BeOfType<EmployeeEducation>();
    }

    [Fact]
    public void Create_ForGivenDateTimeParams_SetsApropriateGraduatedAtProperty()
    {
        // Arrange
        DateTime graduatedAt = new(2025, 12, 12);
        DateOnly expectedGraduatedAt = new(2025, 12, 12);

        // Act
        EmployeeEducation employeeEducation = CreateWithEducationDateTimes(graduatedAt: graduatedAt);

        // Assert
        employeeEducation
            .GraduatedAt
            .Should()
            .Be(expectedGraduatedAt);
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesEmployeeEducationProperties()
    {
        // Arrange
        EducationType educationType = EducationType.Higher;
        string educationDetails = "Education details";
        DateOnly enrolledAt = new(2020, 1, 1);
        DateOnly? graduatedAt = new(2025, 1, 1);

        var employeeEducation = CreateWithEducationDates();
        var expectedEmployeeEducation = CreateWithEducationDates(educationType, educationDetails, enrolledAt, graduatedAt);

        // Act
        EmployeeEducation.Update(employeeEducation, educationType, educationDetails, enrolledAt, graduatedAt);

        // Assert
        employeeEducation
            .Should()
            .BeEquivalentTo(expectedEmployeeEducation, options => options
                .Including(lt => lt.EducationType)
                .Including(lt => lt.EducationDetails)
                .Including(lt => lt.EnrolledAt)
                .Including(lt => lt.GraduatedAt)
            );
    }

    public static IEnumerable<object[]?> GetInvalidDataForEducationDetails() => [[null], [""]];

    #region Test_Factory_Methods

    private static EmployeeEducation CreateWithEducationDates(EducationType educationType = EducationType.Basic,
                                                              string educationDetails = "Details",
                                                              DateOnly enrolledAt = new(),
                                                              DateOnly? graduatedAt = null)
        =>
            EmployeeEducation.Create(EmployeeHelper.CreateEmployee(), educationType, educationDetails, enrolledAt, graduatedAt);

    private static EmployeeEducation CreateWithEducationDateTimes(EducationType educationType = EducationType.Basic,
                                                                  string educationDetails = "Details",
                                                                  DateTime enrolledAt = new(),
                                                                  DateTime? graduatedAt = null)
        =>
            EmployeeEducation.Create(EmployeeHelper.CreateEmployee(), educationType, educationDetails, enrolledAt, graduatedAt);

    #endregion
}
