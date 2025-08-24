using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class DepartmentHelper
{
    internal static async Task<Department> CreateDepartmentWithSectionListAsync(string? departmentName = null,
                                                                                string? dedicatedSectionName = null)
    {
        Department department = await Department.CreateSingleAsync(
            name: departmentName ?? "Department",
            leader: null,
            description: null,
            departmentRuleSet: new Mock<IDepartmentRuleSet>().Object
        );

        department.AddSingleSection(dedicatedSectionName ?? "Section_1", leader: null);
        department.AddSingleSection("Section_2", leader: null);
        department.AddSingleSection("Section_3", leader: null);
        department.AddSingleSection("Section_4", leader: null);
        department.AddSingleSection("Section_5", leader: null);

        return department;
    }
}
