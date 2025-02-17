using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Users;

public partial class List
{
    [Inject]
    private IUserService UserService { get; set; }

    [Inject]
    private IRoleService RoleService { get; set; }

    private MudDataGrid<UserDetailsViewModel> _dataGrid;
    private Column<UserDetailsViewModel> _rolesColumn;

    private string? _selectedRole;
    private string? _searchPhrase;
    private string? searchString1;
    string selectedFilter;

    private bool _isInFavourites;
    private bool _isLoading = true;
    private bool _isChoosenMany = false;
    private bool _isAdvancedFiltering = false;
    bool _filterOpen = false;
    bool _selectAll = true;
    string _icon = Icons.Material.Outlined.FilterAlt;

    private List<UserDetailsViewModel> Users = [];
    private List<string> Roles = [];

    HashSet<string> _selectedItems = [];
    HashSet<string> _filterItems = [];
    FilterDefinition<UserDetailsViewModel> _filterDefinition;

    protected override async Task OnInitializedAsync()
    {
        Users = await UserService.GetAllAsync();

        var roles = await RoleService.GetAllAsync();
        Roles = roles
            .Select(role => role.Name)
            .ToList();

        _selectedItems = Roles.ToHashSet();
        _filterItems = Roles.ToHashSet();

        _isLoading = false;
        StateHasChanged();
    }

    protected override bool ShouldRender() => !_isLoading;

    void OpenFilter()
    {
        _filterOpen = true;
    }

    private void SelectAll(bool value)
    {
        _selectAll = value;

        if (value)
        {
            _selectedItems = Roles.ToHashSet();
        }
        else
        {
            _selectedItems.Clear();
        }
    }

    private void SelectedChanged(bool value, string item)
    {
        if (value)
            _selectedItems.Add(item);
        else
            _selectedItems.Remove(item);

        if (_selectedItems.Count == Roles.Count())
            _selectAll = true;
        else
            _selectAll = false;
    }

    private async Task ClearFilterAsync(FilterContext<UserDetailsViewModel> context)
    {
        _selectedItems = Roles.ToHashSet();
        _filterItems = Roles.ToHashSet();
        _icon = Icons.Material.Outlined.FilterAlt;

        var existingFilter = context.FilterDefinitions
            .FirstOrDefault(f => f.Column == _rolesColumn);

        if (existingFilter is not null)
            await context.Actions.ClearFilterAsync(_filterDefinition);

        _filterOpen = false;
    }

    private async Task ApplyFilterAsync(FilterContext<UserDetailsViewModel> context)
    {
        _filterItems = _selectedItems.ToHashSet();

        _icon = _filterItems.Count == Users.Count()
            ? Icons.Material.Outlined.FilterAlt
            : Icons.Material.Filled.FilterAlt;

        var existingFilter = context.FilterDefinitions
            .FirstOrDefault(f => f.Column == _rolesColumn);

        if (existingFilter is not null)
            await context.Actions.ClearFilterAsync(existingFilter);
        
        _filterDefinition = new FilterDefinition<UserDetailsViewModel>()
        {
            Title = _rolesColumn.Title,
            Column = _rolesColumn,
            Value = string.Join("|", _filterItems),
            Operator = "equals"
        };

        await context.Actions.ApplyFilterAsync(_filterDefinition);

        _filterOpen = false;
    }

    private async Task<GridData<UserDetailsViewModel>> LoadServerData(GridState<UserDetailsViewModel> state)
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        var filterDefinitions = state.FilterDefinitions;
        var pageNumber = state.Page + 1;
        var pageSize = state.PageSize;

        string sieveSort = "Email";
        var sieveFilters = filterDefinitions
            .Select(filter => SieveMapper.MapToSieveFilter(filter.Column.PropertyName, filter));

        var sieveFilterString = string.Join(",", sieveFilters);

        Console.WriteLine(sieveFilterString);

        if (sortDefinition is not null)
        {
            sieveSort = SieveMapper.MapToSieveSort(sortDefinition.SortBy, sortDefinition.Descending);
            Console.WriteLine(sieveSort);
        }

        var data = await UserService.GetAllAsync(pageNumber, pageSize, filters: sieveFilterString, sorts: sieveSort);

        return new GridData<UserDetailsViewModel>
        {
            Items = data.Items,
            TotalItems = data.TotalItemsCount
        };
    }
}
