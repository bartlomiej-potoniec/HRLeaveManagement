using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class Create
{
    [Inject] private ILeaveTypeService LeaveTypeService { get; set; }
    [Inject] private ILeaveRequestService LeaveRequestService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    public LeaveRequestViewModel? LeaveRequest { get; set; } = new();
    public List<LeaveTypeViewModel> LeaveTypes { get; set; } = [];

    protected override async Task OnInitializedAsync()
        => LeaveTypes = (List<LeaveTypeViewModel>)await LeaveTypeService.GetAll();
    
    private async Task HandleValidSubmit()
    {
        var result = await LeaveRequestService.CreateAsync(LeaveRequest!);

        if (!result.IsSuccess)
        {
            Message.HandleError(result.Message);
            return;
        }

        Message.HandleSuccess("Leave request has been sent successfully");
        NavigationManager.NavigateTo("/leave-requests/");
    }
}
