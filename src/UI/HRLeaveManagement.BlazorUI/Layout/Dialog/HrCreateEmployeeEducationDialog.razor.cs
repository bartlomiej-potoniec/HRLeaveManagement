using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrCreateEmployeeEducationDialog : HrDialog
{
    [CascadingParameter] private Message Message { get; set; }

    [Parameter] public string EmployeeFullName { get; set; }
    [Parameter] public EmployeeEducationViewModel Model { get; set; } = new();

    private MudForm Form { get; set; }

    private IViewModelValidator<EmployeeEducationViewModel> Validator  
        => new EmployeeEducationViewModelValidator();

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private void IsEmployeeStillStudyingValueChanged(bool value)
    {
        if (value)
        {
            Model.GraduatedAt = null;
        }

        Model.IsEmployeeStillStudying = value;
    }

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
