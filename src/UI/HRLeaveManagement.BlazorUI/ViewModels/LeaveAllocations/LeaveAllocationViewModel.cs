using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveAllocations;

public class LeaveAllocationViewModel
{
    public int Id { get; set; }
    [Display(Name = "Number Of Days")]

    public int NumberOfDays { get; set; }
    public DateTime DateCreated { get; set; }
    public int Period { get; set; }

    public LeaveTypeViewModel LeaveType { get; set; }
    public int LeaveTypeId { get; set; }
}
