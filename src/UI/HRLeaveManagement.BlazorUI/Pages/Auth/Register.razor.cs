using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Auth;

public partial class Register
{
    [Inject] public IAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    private RegisterViewModel Model { get; set; }
    private IViewModelValidator<RegisterViewModel> Validator => new RegisterViewModelValidator();

    private MudForm Form { get; set; }
    private bool _isLoading = true;

    protected override void OnParametersSet()
    {
        _isLoading = true;

        Model = new RegisterViewModel();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task HandleValidSubmitAsync()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError(Form.Errors);
            return;
        }

        var result = await AuthenticationService.RegisterAsync(
            Model.FirstName,
            Model.LastName,
            Model.Email,
            Model.DateOfBirth!.Value,
            Model.PeselNumber,
            Model.PhoneNumber,
            Model.Roles
        );

        string message = result.Message;

        if (!result.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        Message.HandleSuccess(message);
        NavigationManager.NavigateTo($"/users/{ result.Data.UserId }/details");
    }

    private void Reset()
    {
        Model.FirstName = null;
        Model.LastName = null;
        Model.Email = null;
        Model.DateOfBirth = null;
        Model.PeselNumber = null;
        Model.PhoneNumber = null;
        Model.HasPeselNumber = false;
        Model.Roles = [];
    }

    private async Task HasPeselNumberAsync(bool value)
    {
        if (!value)
        {
            Model.PeselNumber = null;
        }

        Model.HasPeselNumber = value;
        await Form.Validate();
    }
}
