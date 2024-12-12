namespace HRLeaveManagement.Domain.Tests.Entities;

public class EmployeeTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        var employee = CreateWithDefaultValues();

        // Assert
        employee
            .Should()
            .BeOfType<Employee>();
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string position = "Logistic";
        string responsibilities = "Logistics";
        int sectionId = 2;
        Guid leaderId = Guid.NewGuid();

        var employee = CreateWithDefaultValues();
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

    private static Employee CreateWithDefaultValues()
        => Employee.Create("Engineer", "Engineering", 1, Guid.NewGuid());
}