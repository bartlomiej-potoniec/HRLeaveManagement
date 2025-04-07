using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.Users;

public partial class Details
{
    [Inject] public IUserService UserService { get; set; }

    [CascadingParameter] Message Message { get; set; }

    [Parameter] public required string Id { get; set; }

    private UserDetailsViewModel Model { get; set; } = new();

    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        var isValidId = Guid.TryParse(Id, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Podano nieprawidłowy identyfikator");
            return;
        }

        var response = await UserService.GetWithDetailsByIdAsync(id);

        if (!response.IsSuccess)
        {
            Message.HandleError("Nie znaleziono użytkownika o podanym ID");
            return;
        }

        Model = response.Data;

        _isLoading = false;
        StateHasChanged();
    }
}
