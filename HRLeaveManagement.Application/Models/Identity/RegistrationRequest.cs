using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.Application.Models.Identity;

public record RegistrationRequest
{
    [Required]
    public required string FirstName { get; init; }

    [Required]
    public required string LastName { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [MinLength(6)]
    public required string UserName { get; init; }

    [Required]
    [MinLength(6)]
    public required string Password { get; init; }
}
