using HRLeaveManagement.BlazorUI.ViewModels.Roles;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IRoleService
{
    Task<List<RoleViewModel>> GetAllAsync();
}
