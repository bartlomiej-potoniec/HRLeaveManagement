using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveAllocationService(IClient client,
                                           ILocalStorageService localStorage) 
    : HttpServiceBase(client, localStorage), ILeaveAllocationService
{

}
