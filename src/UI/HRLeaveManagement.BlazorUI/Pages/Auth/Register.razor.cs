using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Auth;

public partial class Register
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }

    [Inject]
    public IUserService UserService { get; set; }

    [Inject]
    public IRoleService RoleService { get; set; }

    [CascadingParameter]
    protected Error Error { get; set; }

    private MudForm Form { get; set; }
    public string? Message { get; set; }

    public RegisterViewModel Model { get; set; } = new();
    private RegisterViewModelValidator Validator { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetAllAsync();

        var roles = await RoleService.GetAllAsync();
        Roles = roles
            .Select(role => role.Name)
            .ToList();
    }

    // Existing Users from UserService
    private List<UserDetailsViewModel> Users = [];
    // Existing Role Names from RoleService
    private List<string> Roles = [];

    private bool _hasPesel = true;
    private string HasPeselText => _hasPesel ? "Posiada PESEL" : "Nie posiada PESEL";
    // Aktualnie wybrana wartość z Autocomplete
    private string? SelectedRole;

    // Czy przycisk jest nieaktywny?
    private bool IsAddRoleDisabled => string.IsNullOrWhiteSpace(SelectedRole) || Model.Roles.Contains(SelectedRole);

    // Funkcja do filtrowania dostępnych ról
    private Task<IEnumerable<string>> SearchRoles(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Task.FromResult(Roles.Except(Model.Roles).AsEnumerable());

        // Filtrowanie ról na podstawie wpisanej frazy
        return Task.FromResult(Roles
            .Except(Model.Roles) // Wyklucz już wybrane role
            .Where(r => r.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .AsEnumerable()
        );
    }

    // Obsługa dodawania roli po zatwierdzeniu
    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrEmpty(SelectedRole))
        {
            AddRole(SelectedRole);
            SelectedRole = null;
        }
    }

    private void AddRoleFromButton()
    {
        if (!string.IsNullOrWhiteSpace(SelectedRole))
        {
            AddRole(SelectedRole);
            SelectedRole = null;
        }
    }

    private void AddRole(string role)
    {
        if (!Model.Roles.Contains(role) && Roles.Contains(role))
            Model.Roles.Add(role);
    }

    private void RemoveRole(string role)
    {
        if (Model.Roles.Contains(role))
            Model.Roles.Remove(role);
    }

    private async Task HandleValidSubmit()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message = "Wystąpił błąd w walidacji formularza";
            Error.HandleError(Message);

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

        Message = result.Message;

        if (!result.IsSuccess)
        {
            Error.HandleError(Message);
            return;
        }

        Snackbar.Add(Message, Severity.Success);
        NavigationManager.NavigateTo($"/users/{ result.Data.UserId }/details");
    }
}
