using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Domain.BoundedEntities;

public class DepartmentWithSections(Department department)
{
    internal Department Department => department;
    public IReadOnlyList<Section> Sections => Department.Sections;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="leader"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public Section AddSingleSection(string name, Employee leader, string? description = null)
        => Department.AddSingleSection(name, leader, description);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sectionPayloads"></param>
    /// <returns></returns>
    public IReadOnlyList<Section> AddManySections(IEnumerable<SectionPayload> sectionPayloads)
        => Department.AddManySections(sectionPayloads);
}
