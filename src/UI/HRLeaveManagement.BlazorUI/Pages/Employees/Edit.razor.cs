using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Edit
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

    [Inject]
    private IMapper Mapper { get; set; }

    [Parameter]
    public string Id { get; set; }

    [CascadingParameter]
    private Error Error { get; set; }

    private List<EmployeeViewModel> Leaders { get; set; } = [];
    private List<SectionViewModel> Sections { get; set; } = [];

    private UserDetailsViewModel? User { get; set; }
    private EditEmployeeDetailsViewModel Model { get; set; } = new();

    private MudForm Form { get; set; }
    public string? Message { get; set; }
    private EditEmployeeDetailsViewModelValidator Validator { get; set; } = new();
     
    protected override async Task OnInitializedAsync()
    {
        var employee = (await EmployeeService.GetWithDetailsByIdAsync(Guid.Parse(Id))).Data;
        Model = Mapper.Map<EditEmployeeDetailsViewModel>(employee);

        Model.Educations = [.. Model.Educations.OrderByDescending(e => e.EnrolledAt)];
        Model.Experiences = [.. Model.Experiences.OrderByDescending(e => e.EmployedFrom)];
        Model.Contracts = [.. Model.Contracts.OrderByDescending(e => e.EmployedFrom)];

        User = await UserService.GetWithDetailsByIdAsync(Model.UserId);

        Leaders = (await EmployeeService.GetAllAsync()).Data
            .Where(l => l.IsLeader is not null && l.IsLeader.Value is true)
            .ToList();

        Sections = await SectionService.GetAllAsync();
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

    private async Task AddContractToListDialog()
    {
        var model = new EmployeeContractViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeContractDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        Model.Contracts.Add(model);
        Model.Contracts = [.. Model.Contracts.OrderByDescending(e => e.EmployedFrom)];

        StateHasChanged();
    }

    private async Task UpdateContractInListDialog(EmployeeContractViewModel contract)
    {
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", User?.FullName },
            { "Model", contract }
        };

        var dialog = await DialogService.ShowAsync<CreateEmployeeContractDialog>("Dodawanie danych do listy", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled) return;

        Model.Contracts = [.. Model.Contracts.OrderByDescending(e => e.EmployedFrom)];
        StateHasChanged();
    }

    private void RemoveContractFromListDialog(EmployeeContractViewModel contract)
    {
        Model.Contracts.Remove(contract);
        Model.Contracts = [.. Model.Contracts.OrderByDescending(e => e.EmployedFrom)];

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

        var result = await EmployeeService.UpdateWithDetailsAsync(Model);

        Message = result.Message;

        if (!result.IsSuccess)
        {
            Error.HandleError(Message);
            return;
        }

        Snackbar.Add(Message, Severity.Success);
        NavigationManager.NavigateTo($"/employees/{ Model.EmployeeId }/details");
    }
}
