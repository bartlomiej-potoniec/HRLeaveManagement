namespace HRLeaveManagement.BlazorUI.ViewModels.Roles;

public record RoleViewModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}
