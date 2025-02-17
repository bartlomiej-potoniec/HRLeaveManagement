using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class AnotherEmployeesSection
{
    [Inject]
    private IEmployeeService EmployeeService { get; set; }

    [Parameter]
    public Guid? ActualEmployeeId { get; set; } = null;

    [Parameter]
    public int Height { get; set; } = 350;

    private List<EmployeeViewModel> Employees = [];
    private bool _isLoaded = false;

    protected override async Task OnInitializedAsync()
    {
        Employees = (await EmployeeService.GetAllAsync()).Data;

        _isLoaded = true;
        StateHasChanged();
    }

    protected override bool ShouldRender() => _isLoaded;
}
