using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace HRLeaveManagement.BlazorUI.Layout.Form;

public partial class HrLeaderSelect
{
    [Inject] private IEmployeeService EmployeeService { get; set; }

    [CascadingParameter] protected Message Message { get; set; }

    [Parameter, EditorRequired] public required Guid? Value { get; set; }
    [Parameter, EditorRequired] public required Expression<Func<Guid?>> For { get; set; }
    [Parameter] public EventCallback<Guid?> ValueChanged { get; set; }
    [Parameter] public EventCallback<bool> OnDataLoaded { get; set; }
    [Parameter] public bool IsDisabled { get; set; } = false;

    private List<EmployeeViewModel> Leaders { get; set; } = [];

    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await EmployeeService.GetAllAsync();
        
        if (!response.IsSuccess) 
        {
            Message.HandleError(response.Message);
            return;
        }

        Leaders = response.Data
            .Where(l => l.IsLeader.HasValue && l.IsLeader.Value)
            .ToList();

        _isLoading = false;
        await OnDataLoaded.InvokeAsync(_isLoading);
    }
}
