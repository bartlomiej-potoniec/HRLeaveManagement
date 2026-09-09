using HRLeaveManagement.Domain.Department;
using HRLeaveManagement.Domain.Department.Section;

namespace HRLeaveManagement.Domain.Tests.Helpers;

public class SectionHelper
{
    public static async Task<Section> CreateSection(string name = "Section",
                                                    Department.Department? department = null,
                                                    Employee? leader = null,
                                                    string? description = null)
    {
        department ??= await DepartmentHelper.CreateDepartment();
    }
}
