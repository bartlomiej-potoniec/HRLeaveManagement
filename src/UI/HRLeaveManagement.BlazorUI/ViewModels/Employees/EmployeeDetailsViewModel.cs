using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class EmployeeDetailsViewModel
{
    public Guid UserId { get; set; }
    public Guid EmployeeId { get; set; }

    [DisplayName("Firstname")]
    public string FirstName { get; set; }

    [DisplayName("Lastname")]
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("PESEL number")]
    public string? PeselNumber { get; set; }

    [DisplayName("Phone number")]
    public string PhoneNumber { get; set; }

    [DisplayName("Date of birth")]
    public DateTime DateOfBirth { get; set; }

    [DisplayName("Position")]
    public string Position { get; set; }

    [DisplayName("Section")]
    public string Section { get; set; }
    public int? SectionId { get; set; }

    [DisplayName("Department")]
    public string Department { get; set; }
    public int DepartmentId { get; set; }

    public string SectionFullInfo => $"{Section} / {Department}";

    [DisplayName("Responsibilities")]
    public string Responsibilities { get; set; }

    public bool IsCurrentlyEmployed { get; set; }

    [DisplayName("Contract list")]
    public List<EmployeeContractViewModel> Contracts { get; set; } = [];

    [DisplayName("Education list")]
    public List<EmployeeEducationViewModel> Educations { get; set; } = [];

    [DisplayName("Experience list")]
    public List<EmployeeExperienceViewModel> Experiences { get; set; } = [];

    public Guid? LeaderId { get; set; }

    [DisplayName("Leader name")]
    public string? LeaderName { get; set; }
}
