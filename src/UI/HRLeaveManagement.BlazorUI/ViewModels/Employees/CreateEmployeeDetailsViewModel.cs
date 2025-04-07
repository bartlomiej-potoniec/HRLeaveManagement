using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class CreateEmployeeDetailsViewModel
{
    public Guid? UserId { get; set; }

    [DisplayName("Position")]
    public string Position { get; set; }

    [DisplayName("Responsibilities")]
    public string Responsibilities { get; set; }

    public int? SectionId { get; set; }
    public Guid? LeaderId { get; set; }

    [DisplayName("Contract")]
    public EmployeeContractViewModel Contract { get; set; } = new();

    [DisplayName("Education list")]
    public List<EmployeeEducationViewModel> Educations { get; set; } = [];

    [DisplayName("Experience list")]
    public List<EmployeeExperienceViewModel> Experiences { get; set; } = [];
}
