using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes;

public partial class Index
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ILeaveTypeService LeaveTypeService { get; set; }

    [Inject]
    public ILeaveAllocationService LeaveAllocationService { get; set; }

    [CascadingParameter]
    public Error Error { get; set; }

    public IEnumerable<LeaveTypeViewModel>? LeaveTypes { get; set; } = [];
    public string? Message { get; set; }

    private List<BreadcrumbItem> _items = [
        new BreadcrumbItem("Panel pracownika", href: "#"),
        new BreadcrumbItem("Urlopy", href: "#"),
        new BreadcrumbItem("Lista urlopów", href: null, disabled: true)
    ];

    private bool _isLoading;

    private int _pageId = 1;
    public int PageId => _pageId++;

    protected void CreateLeaveType()
        => NavigationManager.NavigateTo("/leavetypes/create/");

    protected async Task AllocateLeaveType(int id)
    {
        await LeaveAllocationService.CreateLeaveAllocations(id);
    }

    protected void DetailLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/details/{id}");

    protected void EditLeaveType(int id)
        => NavigationManager.NavigateTo($"/leavetypes/edit/{id}");

    protected async Task DeleteLeaveType(int id)
    {
        var response = await LeaveTypeService.Delete(id);
        Message = response.Message;

        if (response.IsSuccess) StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        var leaveTypes = await LeaveTypeService.GetAll();

        if (leaveTypes is null) 
        {
            Error.HandleError("Something went wrong... Please try again later");
            return;
        }

        _isLoading = false;
        LeaveTypes = leaveTypes;
    }
}