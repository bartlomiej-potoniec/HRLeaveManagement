using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class Details
{
    [Inject]
    public ILeaveRequestService LeaveRequestService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public int Id { get; set; }
    
    public LeaveRequestViewModel Model { get; set; } = new();

    private string _headingText = string.Empty;
    private MudBlazor.Color _mudColor = MudBlazor.Color.Primary;

    protected override async Task OnInitializedAsync()
        => Model = await LeaveRequestService.GetByIdAsync(Id);

    protected override void OnParametersSet()
        =>
            (_mudColor, _headingText) = Model.IsApproved switch
            {
                null => (MudBlazor.Color.Warning, "Pending"),
                true => (MudBlazor.Color.Success, "Approved"),
                _    => (MudBlazor.Color.Error, "Rejected")
            };

    private async Task ChangeApproval(bool approvalStatus)
    {
        await LeaveRequestService.ApproveAsync(Id, approvalStatus);
        NavigationManager.NavigateTo("/leave-requests/");
    }
}
