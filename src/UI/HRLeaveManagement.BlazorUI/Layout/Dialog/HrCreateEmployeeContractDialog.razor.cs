using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrCreateEmployeeContractDialog : HrDialog
{
    [Parameter] public string EmployeeFullName { get; set; }
    [Parameter] public EmployeeContractViewModel Model { get; set; }

    [CascadingParameter] private Message Message { get; set; }

    private MudForm Form { get; set; }

    private IViewModelValidator<EmployeeContractViewModel> Validator 
        => new EmployeeContractViewModelValidator();

    protected override async Task Confirm()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError(Form.Errors);
            return;
        }

        MudDialog.Close(DialogResult.Ok(Model));
    }
}
