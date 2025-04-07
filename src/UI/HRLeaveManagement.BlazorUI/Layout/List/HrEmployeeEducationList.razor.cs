using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.List;

public partial class HrEmployeeEducationList : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }

    [Parameter, EditorRequired]
    public required List<EmployeeEducationViewModel> Educations { get; set; } = [];

    [Parameter]
    public required EventCallback<List<EmployeeEducationViewModel>> EducationsChanged { get; set; }

    [Parameter] public string? EmployeeFullName { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Parameter] public bool HasOptionsButtons { get; set; } = false;
    [Parameter] public bool HasOptionsForItems { get; set; } = false;

    private async Task AddEducationToListDialogAsync()
    {
        if (!HasOptionsButtons)
        {
            return;
        }

        var model = new EmployeeEducationViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeEducationDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Educations.Add(model);
        Educations = [.. Educations.OrderByDescending(e => e.EnrolledAt)];

        await EducationsChanged.InvokeAsync(Educations);
        StateHasChanged();
    }

    private async Task UpdateEducationInListDialogAsync(EmployeeEducationViewModel education)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", education }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeEducationDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Educations = [.. Educations.OrderByDescending(e => e.EnrolledAt)];

        await EducationsChanged.InvokeAsync(Educations);
        StateHasChanged();
    }

    private async Task RemoveEducationFromListDialogAsync(EmployeeEducationViewModel education)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        Educations.Remove(education);
        Educations = [.. Educations.OrderByDescending(e => e.EnrolledAt)];

        await EducationsChanged.InvokeAsync(Educations);
        StateHasChanged();
    }
}
