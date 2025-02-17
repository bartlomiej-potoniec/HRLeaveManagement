namespace HRLeaveManagement.Application.DTOs.Roles;

public record RoleDTO
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
