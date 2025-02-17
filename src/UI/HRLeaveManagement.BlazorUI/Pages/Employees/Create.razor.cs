using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Create
{
    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IUserService UserService { get; set; }

    [Inject]
    private IEmployeeService EmployeeService { get; set; }

    [Inject]
    private ISectionService SectionService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [CascadingParameter]
    private Error Error { get; set; }

    private List<UserDetailsViewModel> Users { get; set; } = [];
    private List<EmployeeViewModel> Leaders { get; set; } = [];
    private List<SectionViewModel> Sections { get; set; } = [];

    private UserDetailsViewModel? User { get; set; }
    private CreateEmployeeDetailsViewModel Model { get; set; } = new();

    private MudForm Form { get; set; }
    public string? Message { get; set; }
    private bool _isCheckBoxSelected;
    private EmployeeDetailsViewModelValidator Validator { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Users = (await UserService
            .GetAllAsync(sorts: "EmployeeId", filters: "EmployeeId@=\\null")).Items;

        Leaders = (await EmployeeService.GetAllAsync()).Data
            .Where(l => l.IsLeader is not null && l.IsLeader.Value is true)
            .ToList();

        Sections = await SectionService.GetAllAsync();
    }

    private void SelectedChanged(UserDetailsViewModel? item)
    {
        User = item;
        Model.UserId = item.Id;
    }

    private void SelectedCheckBoxChanged(bool isChecked)
    {
        if (isChecked)
        {
            _isCheckBoxSelected = true;

            if (Model.Contract is not null)
                Model.Contract.EmployedTo = null;

            return;
        }

        _isCheckBoxSelected = false;
    }

    private async Task AddEducationToListDialog()
    {
        var model = new EmployeeEducationViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeEducationDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        Model.Educations.Add(model);
        Model.Educations = [.. Model.Educations.OrderByDescending(e => e.EnrolledAt)];

        StateHasChanged();
    }

    private async Task UpdateEducationInListDialog(EmployeeEducationViewModel education)
    {
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", education }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeEducationDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        Model.Educations = [.. Model.Educations.OrderByDescending(e => e.EnrolledAt)];
        StateHasChanged();
    }

    private void RemoveEducationFromListDialog(EmployeeEducationViewModel education)
    {
        Model.Educations.Remove(education);
        Model.Educations = [.. Model.Educations.OrderByDescending(e => e.EnrolledAt)];

        StateHasChanged(); 
    }

    private async Task AddExperienceToListDialog()
    {
        var model = new EmployeeExperienceViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeExperienceDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;
         
        Model.Experiences.Add(model);
        Model.Experiences = [.. Model.Experiences.OrderByDescending(e => e.EmployedFrom)];

        StateHasChanged();
    }

    private async Task UpdateExperienceInListDialog(EmployeeExperienceViewModel experience)
    {
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", experience }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeExperienceDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        Model.Experiences = [.. Model.Experiences.OrderByDescending(e => e.EmployedFrom)];
        StateHasChanged();
    }

    private void RemoveExperienceFromListDialog(EmployeeExperienceViewModel experience)
    {
        Model.Experiences.Remove(experience);
        Model.Experiences = [.. Model.Experiences.OrderByDescending(e => e.EmployedFrom)];

        StateHasChanged();
    }

    public async Task HandleValidSubmit()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message = $"Wystąpił błąd w walidacji formularza : { string.Join(", ", Form.Errors) }";
            Error.HandleError(Message);

            return;
        }

        var result = await EmployeeService.CreateAsync(Model);

        Message = result.Message;

        if (!result.IsSuccess)
        {
            Error.HandleError(Message);
            return;
        }

        Snackbar.Add(Message, Severity.Success);
        NavigationManager.NavigateTo($"/employees/{ result.Data.EmployeeId }/details");
    }
}
