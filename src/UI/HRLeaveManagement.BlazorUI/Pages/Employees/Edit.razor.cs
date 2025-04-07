using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Edit
{
    [Inject] private IEmployeeService EmployeeService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    
    [CascadingParameter] private Message Message { get; set; }

    [Parameter] public string Id { get; set; }

    private EmployeeDetailsViewModel Model { get; set; } = new();

    private MudForm Form { get; set; }

    private IViewModelValidator<EmployeeDetailsViewModel> Validator 
        => new EditEmployeeDetailsViewModelValidator();

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

        Model.Educations = [.. Model.Educations.OrderByDescending(e => e.EnrolledAt)];
        Model.Experiences = [.. Model.Experiences.OrderByDescending(e => e.EmployedFrom)];
        Model.Contracts = [.. Model.Contracts.OrderByDescending(e => e.StartedAt)];

        _isLoading = false;
        StateHasChanged();
    }

    public async Task HandleValidSubmit()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError(Form.Errors);
            return;
        }

        var result = await EmployeeService.UpdateWithDetailsAsync(Model);
        var message = result.Message;

        if (!result.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        Message.HandleSuccess(message);
        NavigationManager.NavigateTo($"/employees/{ Model.EmployeeId }/details");
    }
}
