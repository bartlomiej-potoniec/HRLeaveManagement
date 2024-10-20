using HRLeaveManagement.BlazorUI.ViewModels.LeaveAllocations;

namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;

public class EmployeeLeaveRequestViewModel
{
    public List<LeaveAllocationViewModel> LeaveAllocations { get; set; } = [];
    public List<LeaveRequestViewModel> LeaveRequests { get; set; } = [];
}
