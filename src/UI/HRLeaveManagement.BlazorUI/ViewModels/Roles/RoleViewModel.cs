using System.ComponentModel;

namespace HRLeaveManagement.BlazorUI.ViewModels.Roles;

public record RoleViewModel
{
    public required Guid Id { get; set; }

    [DisplayName("Name")]
    public required string Name { get; set; }
}
