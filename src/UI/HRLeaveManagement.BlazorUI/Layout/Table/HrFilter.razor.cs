using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Layout.Table;

public abstract partial class HrFilter<T> : ComponentBase where T : class
{
    [Parameter, EditorRequired]
    public required IEnumerable<T> Data { get; set; } = [];

    [Parameter, EditorRequired]
    public required FilterContext<T> Context { get; set; }

    [Parameter, EditorRequired]
    public required Column<T> Column { get; set; }

    [Parameter, EditorRequired]
    public required List<IFilterDefinition<T>> FilterDefinitions { get; set; }

    protected List<string> Items { get; set; } = [];

    protected HashSet<string> SelectedItems { get; set; } = [];

    private bool _isFilterOpened = false;
    private bool _isSelectedAll = true;
    private string _filterIcon = Icons.Material.Outlined.FilterAlt;

    private bool _isFirstLoad = true;

    protected override void OnParametersSet()
    {
        if (Data is null || !Data.Any())
        {
            return;
        }

        LoadItems();
       
        if (_isFirstLoad)
        {
            SelectedItems = [.. Items];
            _isFirstLoad = false;
        }
    }

    protected abstract void LoadItems();

    private void OpenFilter() => _isFilterOpened = true;

    private async Task ApplyFilterAsync() => await ApplyFilterAsync(Context);
    private async Task ClearFilterAsync() => await ClearFilterAsync(Context);

    private void SelectAll(bool value)
    {
        _isSelectedAll = value;
        SelectedItems = value ? [.. Items] : [];
    }

    private void SelectedChanged(bool value, string item)
    {
        if (value)
        {
            SelectedItems.Add(item);
        }

        else
        {
            SelectedItems.Remove(item);
        }

        _isSelectedAll = SelectedItems.Count == Items.Count;
    }

    private async Task ApplyFilterAsync(FilterContext<T> context)
    {
        HashSet<string> filteredItems = [.. SelectedItems];

        _filterIcon = filteredItems.Count == Items.Count
            ? Icons.Material.Outlined.FilterAlt
            : Icons.Material.Filled.FilterAlt;

        var existingFilter = context.FilterDefinitions
            .FirstOrDefault(f => f.Column == Column);

        if (existingFilter is not null)
        {
            await context.Actions.ClearFilterAsync(existingFilter);
        }

        FilterDefinitions = [
            new FilterDefinition<T>
            {
                Title = Column.Title,
                Column = Column,
                Value = string.Join("|", filteredItems),
                Operator = "equals"
            }
        ];

        await context.Actions.ApplyFiltersAsync(FilterDefinitions);

        _isFilterOpened = false;
    }

    private async Task ClearFilterAsync(FilterContext<T> context)
    {
        SelectedItems = [.. Items];
        HashSet<string> filteredItems = [.. Items];

        _isSelectedAll = SelectedItems.Count == Items.Count;
        _filterIcon = Icons.Material.Outlined.FilterAlt;

        var existingFilter = context.FilterDefinitions
            .FirstOrDefault(f => f.Column == Column);

        if (existingFilter is not null)
        {
            await context.Actions.ClearFiltersAsync(FilterDefinitions);
        }

        _isFilterOpened = false;
    }
}
