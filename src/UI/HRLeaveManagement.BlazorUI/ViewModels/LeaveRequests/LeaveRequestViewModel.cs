using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;

public class LeaveRequestViewModel
{
    public int Id { get; set; }

    [DisplayName("Requested At")]
    public DateTime RequestedAt { get; set; }

    [DisplayName("Actioned At")]
    public DateTime ActionedAt { get; set; }

    [DisplayName("Approval State")]
    public bool? IsApproved { get; set; }
    public bool Canceled { get; set; }

    [DisplayName("Leave Type Id")]
    public int LeaveTypeId { get; set; }
    public LeaveTypeViewModel LeaveType { get; set; } 

    public EmployeeViewModel? Employee { get; set; } 

    [DisplayName("Started At")]
    public DateTime StartedAt { get; set; }

    [DisplayName("Ended At")]
    public DateTime EndedAt { get; set; }

    [DisplayName("Comments")]
    public string? RequestComments { get; set; }
}
