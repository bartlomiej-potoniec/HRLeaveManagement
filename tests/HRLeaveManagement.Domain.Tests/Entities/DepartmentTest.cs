using HRLeaveManagement.Domain.Department;
using HRLeaveManagement.Domain.Tests.Helpers;

namespace HRLeaveManagement.Domain.Tests.Entities;

public class DepartmentTest
{
    [Fact]
    public void CreateAsync_ThrowsInvalidOperationException_WhenDepartmentWithGivenNameAlreadyExists()
    {
        // Arrange
        string departmentName = "Department_1";
     
        Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock = CreateDepartmentRuleSetMock();
        SetupIsNameUniqueAsyncToReturnResult(departmentRuleSetMock, result: false);

        string expectedExceptionMessage = "Department with name 'Department_1' already exists";

        // Act
        Action result = async () => await CreateWithDefaultValuesAsync(departmentRuleSetMock, name: departmentName);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task CreateAsync_ForGivenParams_ReturnsNewInstance()
    {
        // Arrange
        Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock = CreateDepartmentRuleSetMock();
        SetupIsNameUniqueAsyncToReturnResult(departmentRuleSetMock, result: true);

        // Act
        Department.Department department = await CreateWithDefaultValuesAsync(departmentRuleSetMock);

        // Assert
        department
            .Should()
            .BeOfType<Department>();
    }

    [Fact]
    public async Task UpdateAsync_ThrowsInvalidOperationException_WhenDepartmentWithGivenNameAlreadyExists()
    {
        // Arrange
        string name = "Department_1";
        string description = "Description for Department_1";
        Employee? leader = null;

        Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock = CreateDepartmentRuleSetMock();
        SetupIsNameUniqueAsyncToReturnResult(departmentRuleSetMock, result: true);

        var departmentToUpdate = await CreateWithDefaultValuesAsync(departmentRuleSetMock);

        SetupIsNameUniqueAsyncToReturnResult(departmentRuleSetMock, result: false);

        string expectedExceptionMessage = "Department with name 'Department_1' already exists";

        // Act
        Action result = async () => await departmentToUpdate
            .UpdateAsync(departmentRuleSetMock.Object, name, leader, description);

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
        string name = "Department_1";
        string description = "Description for Department_1";
        Employee? leader = null;

        Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock = CreateDepartmentRuleSetMock();
        SetupIsNameUniqueAsyncToReturnResult(departmentRuleSetMock, result: true);
          
        var departmentToUpdate = await CreateWithDefaultValuesAsync(departmentRuleSetMock);

        var expectedDepartment = new
        {
            Name = name,
            Description = description,
            Leader = leader
        };

        // Act
        await departmentToUpdate.UpdateAsync(departmentRuleSetMock.Object, name, leader, description);

        // Assert
        departmentToUpdate
            .Should()
            .BeEquivalentTo(expectedDepartment, options => options
                .Including(d => d.Name)
                .Including(d => d.Description)
                .Including(d => d.Leader)
            );
    }

    [Fact]
    public async Task AddSection_ThrowsInvalidOperationException_WhenSectionWithGivenNameAlreadyExists()
    {
        // Arrange
        string departmentName = "Department_1";
        string sectionName = "Section_1";
        string sectionDescription = "Description for Section_1";
        Employee? sectionLeader = null;

        Department.Department departmentWithSections = await DepartmentHelper
            .CreateDepartmentWithSectionListAsync(departmentName, sectionName);

        string expectedExceptionMessage = "Section with name 'Section_1' for department: Department_1 already exists";

        // Act
        Action result = () => departmentWithSections.AddSingleSection(sectionName, sectionLeader, sectionDescription);

        // Assert
        result
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(expectedExceptionMessage);
    }

    [Fact]
    public async Task AddSection_ForGivenSection_PutsNewSectionToDepartmentSectionList()
    {
        // Arrange
        string sectionName = "Section_52";
        string sectionDescription = "Description for Section_1";
        Employee? sectionLeader = null;

        Department.Department departmentWithSections = await DepartmentHelper.CreateDepartmentWithSectionListAsync();

        // Act
        departmentWithSections.AddSingleSection(sectionName, sectionLeader, sectionDescription);

        // Assert
        departmentWithSections.Sections
            .Should()
            .ContainSingle(s => s.Name == sectionName);
    }

    private static Mock<IDepartmentNameUniqueChecker> CreateDepartmentRuleSetMock() => new();

    private static void SetupIsNameUniqueAsyncToReturnResult(Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock, bool result)
        => departmentRuleSetMock
            .Setup(rule => rule.IsEligible(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(result);

    private static async Task<Department.Department> CreateWithDefaultValuesAsync(Mock<IDepartmentNameUniqueChecker> departmentRuleSetMock,
                                                                       string name = "R&D",
                                                                       string? description = "R&D department",
                                                                       Employee? leader = null)
        => await Department.Department.CreateSingleAsync(
            departmentRuleSetMock.Object,
            name,
            leader,
            description
        );
    
}
