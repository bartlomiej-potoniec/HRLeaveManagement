namespace HRLeaveManagement.Domain.Tests.Helpers;

public class SectionHelper
{
    public static async Task<Section> CreateSection(string name = "Section",
                                                    Department? department = null,
                                                    Employee? leader = null,
                                                    string? description = null)
    {
        department ??= await DepartmentHelper.CreateDepartment();
    }
}
