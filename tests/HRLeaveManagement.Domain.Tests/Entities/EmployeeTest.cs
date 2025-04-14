using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_ThrowsArgumentException_WhenPositionIsNullOrEmpty(string? position)
    {
        // Arrange
        var expectedExceptionMessage = "Position for employee cannot be empty";

        // Act
        Action result = () => EmployeeHelper.CreateEmployee(position: position);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_ThrowsArgumentException_WhenResponsibilitiesIsNullOrEmpty(string? responsibilities)
    {
        // Arrange
        var expectedExceptionMessage = "Responsibilities for employee cannot be empty";

        // Act
        Action result = () => EmployeeHelper.CreateEmployee(responsibilities: responsibilities);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Theory]
    [InlineData(-12)]
    [InlineData(0)]
    public void Create_ThrowsArgumentException_WhenSectionIdIsLessThanZero(int sectionId)
    {
        // Arrange
        var expectedExceptionMessage = "Section ID for employee must be greater than zero";

        // Act
        Action result = () => EmployeeHelper.CreateEmployee(sectionId: sectionId);

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        Employee employee = EmployeeHelper.CreateEmployee();

        // Assert
        employee
            .Should()
            .BeOfType<Employee>();
    }

    [Fact]
    public void Update_ThrowsArgumentException_WhenEmployeeIsNull()
    {
        // Arrange
        Employee employee = null;

        var expectedExceptionMessage = "Employee must be included";

        // Act
        Action result = () => Employee.Update(employee, "position", "responsibilities");

        // Assert
        result
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Update_ThrowsInvalidOperationException_WhenLeaderIdIsActualEmployeeId()
    {
        // Arrange
        Guid employeeId = default;
        Employee employee = EmployeeHelper.CreateEmployee();

        var expectedExceptionMessage = "Employee cannot be their own leader";

        // Act
        Action result = () => Employee.Update(employee, "position", "responsibilities", leaderId: employeeId);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string position = "Logistic";
        string responsibilities = "Logistics";
        int sectionId = 2;
        Guid leaderId = Guid.NewGuid();

        var employee = EmployeeHelper.CreateEmployee();
        var expectedEmployee = Employee.Create(position, responsibilities, sectionId, leaderId);

        // Act
        Employee.Update(employee, position, responsibilities, sectionId, leaderId);

        // Assert
        employee
            .Should()
            .BeEquivalentTo(expectedEmployee, options => options
                .Including(e => e.Position)
                .Including(e => e.Responsibilities)
                .Including(e => e.SectionId)
                .Including(e => e.LeaderId)
            );
    }
}