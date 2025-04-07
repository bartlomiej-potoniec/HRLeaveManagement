using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes;

public partial class Index
{
    [Inject] private ILeaveTypeService LeaveTypeService { get; set; }
    [Inject] private ILeaveAllocationService LeaveAllocationService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Error { get; set; }

    public IEnumerable<LeaveTypeViewModel> LeaveTypes { get; set; } = [];

    private bool _isLoading = true;

    protected async Task AllocateLeaveType(int id)
        => await LeaveAllocationService.CreateLeaveAllocations(id);
    
    protected void DetailLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/details/{id}");

    protected void EditLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/edit/{id}");

    protected async Task DeleteLeaveType(int id)
    {
        var response = await LeaveTypeService.Delete(id);

        if (response.IsSuccess)
        {
            StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var leaveTypes = await LeaveTypeService.GetAll();

        if (leaveTypes is null) 
        {
            Error.HandleError("Something went wrong... Please try again later");
            return;
        }

        LeaveTypes = leaveTypes;

        _isLoading = false;
        StateHasChanged();
    }
}