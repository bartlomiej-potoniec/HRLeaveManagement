namespace HRLeaveManagement.Domain.Tests.Entities;

public class SectionTest
{
    [Fact]
    public void Create_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        string name = "Injection Mold Design";
        int departmentId = 1;

        // Act
        var section = Section.Create(name, departmentId);

        // Assert
        section
            .Should()
            .BeOfType<Section>();
    }

    [Fact]
    public void Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string name = "Interiors Design";
        int departmentId = 2;

        var section = Section.Create("Injection Mold Design", 1);
        var expectedSection = Section.Create(name, departmentId);

        // Act
        Section.Update(section, name, departmentId);

        // Assert
        section
            .Should()
            .BeEquivalentTo(expectedSection, options => options
                .Including(s => s.Name)
                .Including(s => s.Description)
                .Including(s => s.DepartmentId)
        );
    }
}
