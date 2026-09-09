using HRLeaveManagement.Domain.Department;
using HRLeaveManagement.Domain.Department.Section;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class SectionTest
{
    [Fact]
    public async Task Update_ThrowsInvalidOperationException_WhenSectionWithGivenNameAlreadyExists()
    {
        // Arrange
        string departmentName = "Department_1";
        string dedicatedSectionName = "Section_52";
        string expectedExceptionMessage = $"Section with name 'Section_52' for department: Department_1 already exists";

        Department.Department department = await DepartmentHelper
            .CreateDepartmentWithSectionListAsync(departmentName, dedicatedSectionName);

        Section sectionToUpdate = department.Sections.First(s => s.Name == dedicatedSectionName);

        // Act
        Action result = () => sectionToUpdate.Update(dedicatedSectionName, department, leader: null, description: null);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task Update_ForGivenParams_UpdatesPropertiesOfExistingInstance()
    {
        // Arrange
        string name = "Section_52";
        string description = "Description for Section_52";
        Employee? leader = null;

        Department.Department department = await DepartmentHelper.CreateDepartmentWithSectionListAsync();
        Section sectionToUpdate = department.Sections.First();

        var expectedSection = new
        {
            Name = name,
            Description = description,
            Leader = leader,
        };

        // Act
        sectionToUpdate.Update(name, department, leader, description);

        // Assert
        sectionToUpdate
            .Should()
            .BeEquivalentTo(expectedSection, options => options
                .Including(s => s.Name)
                .Including(s => s.Description)
                .Including(s => s.Leader)
            );
    }
}
