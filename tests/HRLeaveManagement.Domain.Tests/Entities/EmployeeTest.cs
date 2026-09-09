using HRLeaveManagement.Domain.Employee.Address;
using HRLeaveManagement.Domain.Department.Section;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        string position = "Logistic";
        string responsibilities = "Logstic work";
        Mock<Address> addressMock = new();
        Mock<Section> sectionMock = new();
        Mock<Employee> leaderMock = new();

        // Act
        Employee employee = Employee.Create(
            position,
            responsibilities,
            addressMock.Object,
            sectionMock.Object,
            leaderMock.Object
        );

        // Assert
        employee
            .Should()
            .BeOfType<Employee>();     
    }

    [Fact]
    public void Update_ThrowsInvalidOperationException_WhenLeaderIdIsActualEmployeeId()
    {
        // Arrange
        Mock<Employee> employeeMock = new();
        Mock<Employee> invalidLeaderMock = employeeMock;
        
        string expectedExceptionMessage = "Employee cannot be their own leader";

        // Act
        Action result = () => Employee.Update(
            entity: employeeMock.Object,
            "Logistic",
            "Logistic work",
            new Mock<Address>().Object,
            leader: invalidLeaderMock.Object
        );

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
        Address address = new Mock<Address>().Object;
        Section section = new Mock<Section>().Object;
        Employee leader = new Mock<Employee>().Object;

        Employee employee = Employee.Create(
            "Engineer",
            "Engineering work",
            new Mock<Address>().Object,
            new Mock<Section>().Object,
            new Mock<Employee>().Object
        );

        Employee expectedEmployee = Employee.Create(position, responsibilities, address, section, leader);

        // Act
        Employee.Update(employee, position, responsibilities, address, section, leader);

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