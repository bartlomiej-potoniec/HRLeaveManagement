namespace HRLeaveManagement.BlazorUI.ViewModels;

public class EmployeeViewModel
{
    public Guid UserId { get; set; }
    public Guid EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string Position { get; set; }
    public string Section { get; set; }
    public string Department { get; set; }
    public string Responsibilities { get; set; }

    public bool IsCurrentlyEmployed { get; set; }
    public List<EmployeeContractViewModel> Contracts { get; set; } = [];

    public bool? IsLeader { get; set; }
    public Guid? LeaderId { get; set; }
    public string? LeaderName { get; set; }
}
