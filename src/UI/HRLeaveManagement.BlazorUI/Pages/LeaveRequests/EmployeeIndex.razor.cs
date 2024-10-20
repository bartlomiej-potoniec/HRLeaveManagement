using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class EmployeeIndex
{
    [Inject] 
    public ILeaveRequestService LeaveRequestService { get; set; }
    
    [Inject] 
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    IJSRuntime JSRuntime { get; set; }

    public EmployeeLeaveRequestViewModel Model { get; set; } = new();
    public string? Message { get; set; }

    protected override async Task OnInitializedAsync()
        => Model = await LeaveRequestService.GetForEmployeeAsync();

    public async Task CancelRequestAsync(int id)
    {
        var confirm = await JSRuntime.InvokeAsync<bool>("confirm", "Do you want to cancel this request?");
        if (!confirm) return;

        var response = await LeaveRequestService.Cancel(id);

        if (response.IsSuccess)
        {
            StateHasChanged();
            return;
        }

        Message = response.Message;
    }
}
