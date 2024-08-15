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

    private string? _className;
    private string? _headingText;

    protected override async Task OnInitializedAsync()
        => Model = await LeaveRequestService.GetByIdAsync(Id);

    protected override void OnParametersSet()
        =>
            (_className, _headingText) = Model.IsApproved switch
            {
                null => ("warning", "Pending Approval"),
                true => ("success", "Approved"),
                _    => ("danger", "Rejected")
            };

    private async Task ChangeApproval(bool approvalStatus)
    {
        await LeaveRequestService.ApproveAsync(Id, approvalStatus);
        NavigationManager.NavigateTo("/leave-requests/");
    }
}
