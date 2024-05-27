using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

public class LeaveTypeViewModel
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    [Display(Name = "Default Number Of Days")]
    public required int DefaultDays { get; set; }
}
