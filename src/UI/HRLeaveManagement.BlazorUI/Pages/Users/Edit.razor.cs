using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Users;

public partial class Edit
{
    [Inject] private IUserService UserService { get; set; } 
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter] public required string Id { get; set; }

    private UserDetailsViewModel Model { get; set; } = new();
    private IViewModelValidator<UserDetailsViewModel> Validator 
        => new UserDetailsViewModelValidator();
    
    private MudForm Form { get; set; }

    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        var isValidId = Guid.TryParse(Id, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        var response = await UserService.GetWithDetailsByIdAsync(id);

        if (!response.IsSuccess)
        {
            Message.HandleError("Nie znaleziono użytkownika o podanym ID");
            return;
        }

        Model = response.Data;
        Model.HasPeselNumber = Model.PeselNumber is not null;

        _isLoading = false;
        StateHasChanged();
    }

    private async Task SaveChangesAsync()
    {
        var parameters = new DialogParameters
        {
            { "UserName", Model.UserName }
        };

        var dialog = await DialogService.ShowAsync<HrUpdateUserDialog>("Aktualizacja danych użytkownika", parameters);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await HandleValidSubmitAsync();
        }
    }

    private async Task HandleValidSubmitAsync()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError("Wystąpił błąd w walidacji formularza");
            return;
        }

        var result = await UserService.UpdateAsync(
            Model.Id,
            Model.FirstName,
            Model.LastName,
            Model.Email,
            Model.DateOfBirth.Value,
            Model.PeselNumber,
            Model.PhoneNumber,
            Model.Roles
        );

        var message = result.Message;

        if (!result.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        Message.HandleSuccess(message);
        NavigationManager.NavigateTo($"/users/{ Model.Id }/details");
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
