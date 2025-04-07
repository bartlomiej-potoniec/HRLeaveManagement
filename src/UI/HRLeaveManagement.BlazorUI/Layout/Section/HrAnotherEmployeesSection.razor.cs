using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrAnotherEmployeesSection
{
    [Inject] private IEmployeeService EmployeeService { get; set; }
    
    [CascadingParameter] protected Message Message { get; set; }

    [Parameter] public string? ActualEmployeeId { get; set; } = null;
    [Parameter] public string Height { get; set; } = "350px";

    private Guid? _actualEmployeeId = default;

    private List<EmployeeViewModel> Employees = [];
    private bool _isLoaded = false;

    protected override async Task OnParametersSetAsync()
    {
        if (ActualEmployeeId is not null)
        {
            var isValidId = Guid.TryParse(ActualEmployeeId, out Guid id);

            if (!isValidId)
            {
                Message.HandleError("Invalid ID has been provided");
                return;
            }

            _actualEmployeeId = id;
        }

        var response = await EmployeeService.GetAllAsync();

        if (!response.IsSuccess)
        {
            Message.HandleError("Unable to fetch the employees");
            return;
        }

        Employees = response.Data;

        _isLoaded = true;
        StateHasChanged();
    }
}
