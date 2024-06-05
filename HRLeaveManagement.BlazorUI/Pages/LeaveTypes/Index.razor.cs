using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes;

public partial class Index
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ILeaveTypeService LeaveTypeService { get; set; }

    public IEnumerable<LeaveTypeViewModel>? LeaveTypes { get; set; }
    public string? Message { get; set; }


    private int _pageId = 1;
    public int PageId => _pageId++;

    protected void CreateLeaveType()
        => NavigationManager.NavigateTo("/leavetypes/create/");
    
    protected async Task AllocateLeaveType(int id)
    {

    }

    protected void DetailLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/details/{ id }");

    protected void EditLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/edit/{ id }");

    protected async Task DeleteLeaveType(int id)
    {
        var response = await LeaveTypeService.Delete(id);
        Message = response.Message;

        if (response.IsSuccess) StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
        => LeaveTypes = await LeaveTypeService.GetAll();
}