using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PeselNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }

    public Guid? EmployeeId { get; set; }
}
