using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels.Roles;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IRoleService
{
    Task<Response<List<RoleViewModel>>> GetAllAsync();
}
