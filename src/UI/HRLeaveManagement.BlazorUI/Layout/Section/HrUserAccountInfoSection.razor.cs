using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrUserAccountInfoSection
{
    [Inject] private IUserService UserService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter] public required string UserId { get; set; }
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? MinHeight { get; set; }

    private UserDetailsViewModel Model { get; set; } = new();
    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        _isLoading = true;

        var isValidId = Guid.TryParse(UserId, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        var response = await UserService.GetWithDetailsByIdAsync(id);
        var message = response.Message;

        if (!response.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        Model = response.Data;

        _isLoading = false;
        StateHasChanged();
    }
}
