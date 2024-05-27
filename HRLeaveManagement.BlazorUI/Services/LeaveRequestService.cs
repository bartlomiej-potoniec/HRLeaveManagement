using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Services;

public class LeaveRequestService(IClient client) : HttpServiceBase(client), ILeaveRequestService
{

}
