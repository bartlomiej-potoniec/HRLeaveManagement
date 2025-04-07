using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Layout;
using HRLeaveManagement.BlazorUI.Validation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Create
{
    [Inject] private IEmployeeService EmployeeService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter] private Message Message { get; set; }
    
    private CreateEmployeeDetailsViewModel Model { get; set; } = new();
    private UserDetailsViewModel? User { get; set; } = default;

    private MudForm Form { get; set; }
    private IViewModelValidator<CreateEmployeeDetailsViewModel> Validator 
        => new EmployeeDetailsViewModelValidator();

    private bool _isLoading = true;
    private TaskCompletionSource<bool> _leaderTask = new();
    private TaskCompletionSource<bool> _sectionTask = new();

    private void LeaderLoaded(bool isLoaded) => _leaderTask.TrySetResult(isLoaded);
    private void SectionLoaded(bool isLoaded) => _sectionTask.TrySetResult(isLoaded);

    protected override async Task OnInitializedAsync()
    {
        await Task.WhenAll(_leaderTask.Task, _sectionTask.Task);

        _isLoading = false;
        StateHasChanged();
    }

    public async Task HandleValidSubmit()
    {
        await Form.Validate();

        if (!Form.IsValid)
        {
            Message.HandleError(Form.Errors);
            return;
        }

        var result = await EmployeeService.CreateAsync(Model);
        var message = result.Message;

        if (!result.IsSuccess)
        {
            Message.HandleError(message);
            return;
        }

        Message.HandleSuccess(message);
        NavigationManager.NavigateTo($"/employees/{ result.Data.EmployeeId }/details");
    }
}
