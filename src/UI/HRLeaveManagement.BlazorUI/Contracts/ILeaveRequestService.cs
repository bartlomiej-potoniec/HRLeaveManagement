using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ILeaveRequestService
{
    Task<AdminLeaveRequestViewModel> GetForAdminAsync();
    Task<EmployeeLeaveRequestViewModel> GetForEmployeeAsync();
    Task<LeaveRequestViewModel> GetByIdAsync(int id);
    Task<Response> CreateAsync(LeaveRequestViewModel leaveRequest);
    Task DeleteAsync(int id);
    Task<Response> ApproveAsync(int id, bool approvalStatus);
    Task<Response> Cancel(int id);
}
