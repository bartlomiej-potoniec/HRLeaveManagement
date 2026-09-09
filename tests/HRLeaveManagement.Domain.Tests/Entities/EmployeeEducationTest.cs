using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeEducationTest
{
    [Fact]
    public void EmployeeAddEducation_ThrowsInvalidOperationException_WhenGraduatedAtIsFewerThanEnrolledAt()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        DateOnly enrolledAt = new(2021, 1, 1);
        DateOnly graduatedAt = new(2020, 1, 1);

        var expectedExceptionMessage = "Education graduation date must be greater than enroll date";

        // Act
        Action result = () => employee.AddEducation(
            EducationType.Higher,
            "High School of Engineering",
            enrolledAt,
            graduatedAt
        );

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void EmployeeAddEducation_ForGivenEmploymentDateRange_SetsAppropriateTotalDurationValue()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployee();

        EducationType educationType = EducationType.Higher;
        string institutionName = "High School of Engineering";
        string educationDetails = "Education details";
        
        DateOnly enrolledAt = new(2020, 1, 1);
        DateOnly graduatedAt = new(2021, 1, 1);
        int expectedTotalDuartion = 366;

        // Act
        employee.AddEducation(educationType, institutionName, enrolledAt, graduatedAt, educationDetails);

        EmployeeEducation employeeEducation = employee.EmployeeEducations.First();

        // Assert
        employeeEducation.TotalDuration
            .Should()
            .Be(expectedTotalDuartion);
    }

    [Fact]
    public void Update_ForGivenDates_SetsAppropriateTotalDurationtValue()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithEducationList(
            (new DateOnly(2020, 1, 1), new DateOnly(2025, 1, 1))    
        );

        DateOnly enrolledAt = new(2020, 1, 1);
        DateOnly? graduatedAt = new(2025, 1, 1);
        int expectedTotalDuration = 1827;

        var employeeEducation = employee.EmployeeEducations.First();

        // Act
        employeeEducation.Update(
            EducationType.Higher,
            "High School of Engineering",
            enrolledAt,
            graduatedAt,
            "Education details"
        );

        // Assert
        employeeEducation.EnrolledAt
            .Should()
            .Be(enrolledAt);

        employeeEducation.GraduatedAt
            .Should()
            .Be(graduatedAt);

        employeeEducation.TotalDuration
            .Should()
            .Be(expectedTotalDuration);
    }


    [Fact]
    public async Task AddDocument_AddsEmployeeDocumentInstanceToEmployeeDocumentList()
    {
        // Arrange
        Employee employee = EmployeeHelper.CreateEmployeeWithEducationList(
            (new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1))
        );

        Mock<IEmployeeDocumentNumberUniqueChecker> employeeDocumentRuleSetMock = EmployeeDocumentHelper.CreateEmployeeDocumentRuleSetMock();
        EmployeeDocumentHelper.SetupIsDocumentNumberUniqueAsyncToReturnValue(employeeDocumentRuleSetMock, isRuleFailed: false);

        EmployeeEducation employeeEducation = employee.EmployeeEducations.First();
        EmployeeDocument employeeDocument = await EmployeeDocumentHelper.CreateEmployeeDocumentAsync(employeeDocumentRuleSetMock.Object);

        // Act
        employeeEducation.AddDocument(employeeDocument);

        // Assert
        employeeEducation
            .EmployeeDocuments
            .Should()
            .Contain(employeeDocument);
    }
}
