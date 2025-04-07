using HRLeaveManagement.BlazorUI.Contracts;
using Microsoft.AspNetCore.Components;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Details
{
    [Inject] private IEmployeeService EmployeeService { get; set; }
    [Inject] private IUserService UserService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter] public string Id { get; set; }

    private EmployeeDetailsViewModel Model { get; set; } = new();

    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        var isValidId = Guid.TryParse(Id, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Cannot convert given employee ID");
            return;
        }

        var response = await EmployeeService.GetWithDetailsByIdAsync(id);

        if (!response.IsSuccess)
        {
            Message.HandleError("Cannot find an employee with given ID");
            return;
        }

        Model = response.Data;

        Model.Educations = [.. Model.Educations.OrderByDescending(x => x.EnrolledAt)];
        Model.Experiences = [.. Model.Experiences.OrderByDescending(x => x.EmployedFrom)];
        Model.Contracts = [.. Model.Contracts.OrderByDescending(x => x.StartedAt)];

        _isLoading = false;
        StateHasChanged();
    }
}
