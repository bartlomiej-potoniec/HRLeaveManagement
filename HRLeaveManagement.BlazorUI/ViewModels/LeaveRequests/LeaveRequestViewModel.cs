using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    [Required]
    [DisplayName("Leave Type Id")]
    public int LeaveTypeId { get; set; }
    public LeaveTypeViewModel LeaveType { get; set; } 

    public EmployeeViewModel? Employee { get; set; } 

    [Required]
    [DisplayName("Started At")]
    public DateTime StartedAt { get; set; }

    [Required]
    [DisplayName("Ended At")]
    public DateTime EndedAt { get; set; }

    [DisplayName("Comments")]
    [MaxLength(300)]
    public string? RequestComments { get; set; }
}
