using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using HRLeaveManagement.BlazorUI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.Employees;

public partial class Details
{
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Inject]
    private IUserService UserService { get; set; }

    [Inject]
    private IEmployeeService EmployeeService { get; set; }


    [Parameter]
    public string Id { get; set; }

    private UserDetailsViewModel User { get; set; } = new();
    private EmployeeDetailsViewModel Model { get; set; } = new();


    protected override async Task OnInitializedAsync()
    {
        Model = (await EmployeeService.GetWithDetailsByIdAsync(Guid.Parse(Id))).Data;
        User = await UserService.GetWithDetailsByIdAsync(Model.UserId);

        Model.Educations = [.. Model.Educations.OrderByDescending(x => x.EnrolledAt)];
        Model.Experiences = [.. Model.Experiences.OrderByDescending(x => x.EmployedFrom)];
        Model.Contracts = [.. Model.Contracts.OrderByDescending(x => x.StartedAt)];
    }
}
