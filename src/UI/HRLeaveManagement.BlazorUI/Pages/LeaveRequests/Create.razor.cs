using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests;

public partial class Create
{
    [Inject] 
    public ILeaveTypeService LeaveTypeService { get; set; }
    
    [Inject] 
    public ILeaveRequestService LeaveRequestService { get; set; }
    
    [Inject] 
    public NavigationManager NavigationManager { get; set; }

    public LeaveRequestViewModel? LeaveRequest { get; set; } = new();
    public List<LeaveTypeViewModel> LeaveTypes { get; set; } = [];

    public string? Message { get; set; }

    protected override async Task OnInitializedAsync()
        => LeaveTypes = (List<LeaveTypeViewModel>)await LeaveTypeService.GetAll();
    
    private async Task HandleValidSubmit()
    {
        var result = await LeaveRequestService.CreateAsync(LeaveRequest!);

        if (result.IsSuccess)
            NavigationManager.NavigateTo("/leave-requests/");

        Message = result.Message;
    }
}
