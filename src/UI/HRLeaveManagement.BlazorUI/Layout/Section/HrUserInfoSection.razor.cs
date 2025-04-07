using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrUserInfoSection
{
    [Inject] private IUserService UserService { get; set; }
    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required string? UserId { get; set; }
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? MinHeight { get; set; }

    private UserDetailsViewModel? User { get; set; } = default;

    private bool _isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        if (UserId is null)
        {
            User = null;
            
            _isLoading = false;
            StateHasChanged();
            
            return;
        }

        _isLoading = true;

        var isValidId = Guid.TryParse(UserId, out Guid id);

        if (!isValidId)
        {
            Message.HandleError("Invalid ID has been provided");
            return;
        }

        var response = await UserService.GetWithDetailsByIdAsync(id);

        if (!response.IsSuccess)
        {
            Message.HandleError(response.Message);
            return;
        }

        User = response.Data;
        
        _isLoading = false;
        StateHasChanged();
    }
}
