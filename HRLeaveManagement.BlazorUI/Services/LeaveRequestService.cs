using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveRequestService(IClient client,
                                        ILocalStorageService localStorage) 
    : HttpServiceBase(client, localStorage), ILeaveRequestService
{

}
