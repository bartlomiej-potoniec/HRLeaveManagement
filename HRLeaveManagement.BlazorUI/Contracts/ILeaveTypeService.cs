using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface ILeaveTypeService
{
    Task<IEnumerable<LeaveTypeViewModel>> GetAll();
    Task<LeaveTypeViewModel> GetDetails();

    Task<Response<Guid>> Create(LeaveTypeViewModel leaveType);
    Task<Response<Guid>> Update(int id, LeaveTypeViewModel leaveType);
    Task<Response<Guid>> Delete(int id);
}