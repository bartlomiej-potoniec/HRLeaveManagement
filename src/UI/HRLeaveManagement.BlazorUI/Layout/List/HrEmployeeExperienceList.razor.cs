using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.List;

public partial class HrEmployeeExperienceList : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }

    [Parameter, EditorRequired]
    public required List<EmployeeExperienceViewModel> Experiences { get; set; } = [];
    
    [Parameter]
    public EventCallback<List<EmployeeExperienceViewModel>> ExperiencesChanged { get; set; }

    [Parameter] public string? EmployeeFullName { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Parameter] public bool HasOptionsButtons { get; set; } = false;
    [Parameter] public bool HasOptionsForItems { get; set; } = false;

    private async Task AddExperienceToListDialogAsync()
    {
        if (!HasOptionsButtons)
        {
            return;
        }

        var model = new EmployeeExperienceViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeExperienceDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Experiences.Add(model);
        Experiences = [.. Experiences.OrderByDescending(e => e.EmployedFrom)];

        await ExperiencesChanged.InvokeAsync(Experiences);
        StateHasChanged();
    }

    private async Task UpdateExperienceInListDialogAsync(EmployeeExperienceViewModel experience)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", experience }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeExperienceDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Experiences = [.. Experiences.OrderByDescending(e => e.EmployedFrom)];
        StateHasChanged();
    }

    private async Task RemoveExperienceFromListDialogAsync(EmployeeExperienceViewModel experience)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        Experiences.Remove(experience);
        Experiences = [.. Experiences.OrderByDescending(e => e.EmployedFrom)];

        await ExperiencesChanged.InvokeAsync(Experiences);
        StateHasChanged();
    }
}
