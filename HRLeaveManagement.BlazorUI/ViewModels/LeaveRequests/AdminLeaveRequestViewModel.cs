namespace HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;

public class AdminLeaveRequestViewModel
{
    public int TotalRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int PendingRequests { get; set; }
    public int RejectedRequests { get; set; }
    public List<LeaveRequestViewModel> LeaveRequests { get; set; } = [];
}
