using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Users;

public partial class List
{
    [Inject] private IUserService UserService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    private MudDataGrid<UserDetailsViewModel> _dataGrid;
    private Column<UserDetailsViewModel> _rolesColumn;
    private Column<UserDetailsViewModel> _accountStatusColumn;

    private List<UserDetailsViewModel> AllData = [];

    private string? _searchString;
    private bool _isChoosenMany = false;

    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await UserService.GetAllAsync(CancellationToken.None);

        if (!response.IsSuccess)
        {
            Message.HandleError("Unable to fetch the users");
            return;
        }

        AllData = response.Data;

        _isLoading = false;
        StateHasChanged();
    }

    private async Task<GridData<UserDetailsViewModel>?> LoadServerData(GridState<UserDetailsViewModel> state)
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        var filterDefinitions = state.FilterDefinitions;
        var pageNumber = state.Page + 1;
        var pageSize = state.PageSize;
        var sieveSort = "Email";

        var sieveFilters = filterDefinitions
            .Select(filter => SieveMapper.MapToSieveFilter(filter.Column.PropertyName, filter));

        var sieveFilterString = string.Join(",", sieveFilters);

        if (sortDefinition is not null)
        {
            sieveSort = SieveMapper.MapToSieveSort(sortDefinition.SortBy, sortDefinition.Descending);
        }

        var response = await UserService.GetAllAsync(pageNumber, pageSize, filters: sieveFilterString, sorts: sieveSort);

        if (!response.IsSuccess)
        {
            Message.HandleError("Unable to fetch the users");
            return null;
        }

        _isLoading = false;
        StateHasChanged();

        return new GridData<UserDetailsViewModel>
        {
            Items = response.Data.Items,
            TotalItems = response.Data.TotalItemsCount
        };
    }
}
