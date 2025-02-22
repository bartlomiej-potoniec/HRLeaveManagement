using HRLeaveManagement.Application.DTOs.Roles;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync(CancellationToken cancellationToken); 
}
