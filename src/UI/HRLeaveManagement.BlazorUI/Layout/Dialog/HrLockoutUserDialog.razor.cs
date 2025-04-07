using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Dialog;

public partial class HrLockoutUserDialog : HrDialog
{
    [CascadingParameter] private Message Message { get; set; }
    
    [Parameter] public required UserLockoutViewModel Model { get; set; }
    [Parameter] public string? UserName { get; set; }

    private MudForm Form { get; set; }
    private MudDatePicker DatePicker { get; set; }
    private MudTimePicker TimePicker { get; set; }

    private IViewModelValidator<UserLockoutViewModel> Validator 
        => new UserLockoutViewModelValidator();

    private bool _isForIndefinitePeriod;

    private void IsForIndefinitePeriodValueChanged(bool value)
    {
        _isForIndefinitePeriod = value;

        if (_isForIndefinitePeriod)
        {
            Model.LockoutDateEnd = DateTime.MaxValue;
            Model.LockoutTimeEnd = TimeSpan.Zero;
            DatePicker.Disabled = true;
            TimePicker.Disabled = true;

            return;
        }

        Model.LockoutDateEnd = null;
        Model.LockoutTimeEnd = null;
        DatePicker.Disabled = false;
        TimePicker.Disabled = false;
    }

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
