using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrCreateEmployeeExperienceDialog : HrDialog
{
    [CascadingParameter] private Message Message { get; set; }

    [Parameter] public string EmployeeFullName { get; set; }
    [Parameter] public EmployeeExperienceViewModel Model { get; set; } = new();

    private MudForm Form { get; set; }

    private IViewModelValidator<EmployeeExperienceViewModel> Validator 
        => new EmployeeExperienceViewModelValidator();

    protected override async Task Confirm()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError("A form validation error occurred");
            return;
        }

        MudDialog.Close(DialogResult.Ok(Model));
    }
}
