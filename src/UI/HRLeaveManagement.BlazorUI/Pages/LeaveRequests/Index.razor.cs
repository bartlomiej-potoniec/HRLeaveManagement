using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class Index
{
    [Inject]
    private ILeaveRequestService LeaveRequestService { get; set; }

    private bool _isLoading;

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public AdminLeaveRequestViewModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        Model = await LeaveRequestService.GetForAdminAsync();
        _isLoading = false;
    }

    private void GoToDetails(int id)
        => NavigationManager.NavigateTo($"/leave-requests/details/{ id }");
}
