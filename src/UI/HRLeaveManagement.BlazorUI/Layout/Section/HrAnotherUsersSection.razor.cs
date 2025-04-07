using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout.Section;

public partial class HrAnotherUsersSection
{
    [Inject] private IUserService UserService { get; set; }

    [CascadingParameter] public Message Message { get; set; }

    [Parameter] public string? ActualUserId { get; set; } = default;
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? MinHeight { get; set; }

    private Guid? _actualUserId = default;
    
    private List<UserDetailsViewModel> Users { get; set; } = [];    
    private bool _isLoaded = false;

    protected override async Task OnInitializedAsync()
    {
        if (ActualUserId is not null)
        {
            var isValidId = Guid.TryParse(ActualUserId, out Guid id);

            if (!isValidId)
            {
                Message.HandleError("Invalid ID has been provided");
                return;
            }
        
            _actualUserId = id;
        }
            
        var response = await UserService.GetAllAsync(CancellationToken.None);

        if (!response.IsSuccess)
        {
            Message.HandleError("No users to display");
            return;
        }

        Users = response.Data;

        _isLoaded = true;
        StateHasChanged();
    }
}
