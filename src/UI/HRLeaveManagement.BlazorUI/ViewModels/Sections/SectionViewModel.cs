namespace HRLeaveManagement.BlazorUI.ViewModels.Sections;

public class SectionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string DepartmentName { get; set; }
    public Guid? LeaderId { get; set; }
}
