using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Sections;

public class SectionViewModel
{
    public int Id { get; set; }

    [DisplayName("Name")]
    public string Name { get; set; }

    [DisplayName("Description")]
    public string? Description { get; set; }

    [DisplayName("Department name")]
    public string DepartmentName { get; set; }

    public Guid? LeaderId { get; set; }
}
