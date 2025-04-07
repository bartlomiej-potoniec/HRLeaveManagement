using HRLeaveManagement.BlazorUI.Layout.Dialog;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.List;

public partial class HrEmployeeContractList
{
    [Inject] private IDialogService DialogService { get; set; }

    [Parameter, EditorRequired]
    public required List<EmployeeContractViewModel> Contracts { get; set; } = [];

    [Parameter]
    public EventCallback<List<EmployeeContractViewModel>> ContractsChanged { get; set; }

    [Parameter] public string? EmployeeFullName { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;
    [Parameter] public bool HasOptionsButtons { get; set; } = false;
    [Parameter] public bool HasOptionsForItems { get; set; } = false;

    private async Task AddContractToListDialogAsync()
    {
        if (!HasOptionsButtons)
        {
            return;
        }

        var model = new EmployeeContractViewModel();
        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", model }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeContractDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Contracts.Add(model);
        Contracts = [.. Contracts.OrderByDescending(e => e.StartedAt)];

        await ContractsChanged.InvokeAsync(Contracts);
        StateHasChanged();
    }

    private async Task UpdateContractInListDialogAsync(EmployeeContractViewModel contract)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        var parameters = new DialogParameters
        {
            { "EmployeeFullName", EmployeeFullName },
            { "Model", contract }
        };

        var dialog = await DialogService.ShowAsync<HrCreateEmployeeContractDialog>("Adding data to the list", parameters);
        var result = await dialog.Result;

        if (result is null || result.Canceled)
        {
            return;
        }

        Contracts = [.. Contracts.OrderByDescending(e => e.StartedAt)];

        await ContractsChanged.InvokeAsync(Contracts);
        StateHasChanged();
    }

    private async void RemoveContractFromListDialogAsync(EmployeeContractViewModel contract)
    {
        if (!HasOptionsForItems)
        {
            return;
        }

        Contracts.Remove(contract);
        Contracts = [.. Contracts.OrderByDescending(e => e.StartedAt)];

        await ContractsChanged.InvokeAsync(Contracts);
        StateHasChanged();
    }
}
