using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Users;

public partial class Details
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }

    [Inject]
    public IUserService UserService { get; set; }

    [Parameter]
    public string Id { get; set; }

    [CascadingParameter]
    protected Error Error { get; set; }

    private bool _isLoaded = false;

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetAllAsync();
        Model = await UserService.GetWithDetailsByIdAsync(Guid.Parse(Id));

        _isLoaded = true;
        StateHasChanged();
    }

    protected override bool ShouldRender() => _isLoaded;                          

    // Existing Users from UserService
    private List<UserDetailsViewModel> Users = [];
    private UserDetailsViewModel Model { get; set; } = new();

}
