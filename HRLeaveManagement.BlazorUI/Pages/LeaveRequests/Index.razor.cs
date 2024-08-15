using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class Index
{
    [Inject]
    private ILeaveRequestService LeaveRequestService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public AdminLeaveRequestViewModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
        => Model = await LeaveRequestService.GetForAdminAsync();

    private void GoToDetails(int id)
        => NavigationManager.NavigateTo($"/leave-requests/details/{ id }");
}
