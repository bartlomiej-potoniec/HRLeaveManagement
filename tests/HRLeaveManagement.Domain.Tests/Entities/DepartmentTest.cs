namespace HRLeaveManagement.Domain.Tests.Entities;

public class DepartmentTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Act
        var department = CreateWithDefaultValues();

        // Assert
        department
            .Should()
            .BeOfType<Department>();
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string name = "Logistic";
        Guid leaderId = Guid.NewGuid();

        var department = CreateWithDefaultValues();
        var expectedDepartment = Department.Create(name, leaderId);

        // Act
        Department.Update(department, name, leaderId);

        // Assert
        department
            .Should()
            .BeEquivalentTo(expectedDepartment, options => options
                .Including(d => d.Name)
                .Including(d => d.Description)
                .Including(d => d.LeaderId)
            );
    }

    private static Department CreateWithDefaultValues() => Department.Create("R&D", Guid.NewGuid());
}
