using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Services;

public class LeaveAllocationService(IClient client) : HttpServiceBase(client), ILeaveAllocationService
{

}
