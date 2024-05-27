using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

namespace HRLeaveManagement.BlazorUI.Services;

public class LeaveTypeService(IClient client) : HttpServiceBase(client), ILeaveTypeService
{
    public Task<Response<Guid>> Create(LeaveTypeViewModel leaveType)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Guid>> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<LeaveTypeViewModel>> GetAll()
    {
        //var leaveTypes = await _client.all
        throw new NotImplementedException();
    }

    public Task<LeaveTypeViewModel> GetDetails()
    {
        throw new NotImplementedException();
    }

    public Task<Response<Guid>> Update(int id, LeaveTypeViewModel leaveType)
    {
        throw new NotImplementedException();
    }
}
