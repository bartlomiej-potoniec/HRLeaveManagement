namespace HRLeaveManagement.BlazorUI.ViewModels;

public class CreateEmployeeDetailsViewModel
{
    public Guid? UserId { get; set; }
    public string Position { get; set; }
    public string Responsibilities { get; set; }
    public int? SectionId { get; set; }
    public Guid? LeaderId { get; set; }

    public EmployeeContractViewModel Contract { get; set; } = new();
    public List<EmployeeEducationViewModel> Educations { get; set; } = [];
    public List<EmployeeExperienceViewModel> Experiences { get; set; } = [];
}
