using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IEmployeeService
{
    Task<Response<List<EmployeeViewModel>>> GetAllAsync();
    Task<Response<EmployeeDetailsViewModel>> GetWithDetailsByIdAsync(Guid id);
    Task<Response<EmployeeViewModel>> CreateAsync(CreateEmployeeDetailsViewModel viewModel);
    Task<Response> UpdateWithDetailsAsync(EditEmployeeDetailsViewModel viewModel);
}
