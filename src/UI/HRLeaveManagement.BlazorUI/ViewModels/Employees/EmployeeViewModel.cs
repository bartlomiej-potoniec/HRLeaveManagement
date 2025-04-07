using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Employees;

public class EmployeeViewModel
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

    [DisplayName("Phone number")]
    public string PhoneNumber { get; set; }

    [DisplayName("Position")]
    public string Position { get; set; }

    [DisplayName("Section")]
    public string Section { get; set; }

    [DisplayName("Department")]
    public string Department { get; set; }

    [DisplayName("Responsibilities")]
    public string Responsibilities { get; set; }

    public bool IsCurrentlyEmployed { get; set; }
    public List<EmployeeContractViewModel> Contracts { get; set; } = [];

    public bool? IsLeader { get; set; }
    public Guid? LeaderId { get; set; }

    [DisplayName("Leader name")]
    public string? LeaderName { get; set; }
}
