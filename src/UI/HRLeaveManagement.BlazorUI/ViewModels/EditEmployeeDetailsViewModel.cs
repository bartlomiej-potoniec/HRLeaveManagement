namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EditEmployeeDetailsViewModel
{
    public Guid UserId { get; set; }
    public Guid EmployeeId { get; set; }
    public string Position { get; set; }
    public string Responsibilities { get; set; }
    public int? SectionId { get; set; }
    public Guid? LeaderId { get; set; }

    public List<EmployeeContractViewModel> Contracts { get; set; } = [];
    public List<EmployeeEducationViewModel> Educations { get; set; } = [];
    public List<EmployeeExperienceViewModel> Experiences { get; set; } = [];
}
