using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ILeaveTypeService
{
    Task<IEnumerable<LeaveTypeViewModel>> GetAll();
    Task<LeaveTypeViewModel> GetDetails(int id);

    Task<Response> Create(LeaveTypeViewModel leaveType);
    Task<Response> Update(int id, LeaveTypeViewModel leaveType);
    Task<Response> Delete(int id);
}