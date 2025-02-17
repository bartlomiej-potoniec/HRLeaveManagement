namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeDetailsViewModel
{
    public Guid UserId { get; set; }
    public Guid EmployeeId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PeselNumber { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public string Position { get; set; }
    public int SectionId { get; set; }
    public string Section { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; }
    public string Responsibilities { get; set; }

    public bool IsCurrentlyEmployed { get; set; }
    public List<EmployeeContractDetailsViewModel> Contracts { get; set; } = [];
    public List<EmployeeEducationDetailsViewModel> Educations { get; set; } = [];
    public List<EmployeeExperienceDetailsViewModel> Experiences { get; set; } = [];

    public Guid? LeaderId { get; set; }
    public string? LeaderName { get; set; }
}
