using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class AnotherUsersSection
{
    [Inject]
    private IUserService UserService { get; set; }

    [Parameter]
    public Guid? ActualUserId { get; set; } = null;

    [Parameter]
    public int Height { get; set; } = 350;
    
    private List<UserDetailsViewModel> Users = [];    
    private bool _isLoaded = false;

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetAllAsync();

        _isLoaded = true;
        StateHasChanged();
    }

    protected override bool ShouldRender() => _isLoaded;
}
