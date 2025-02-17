using HRLeaveManagement.Application.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PeselNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }

    public Guid? EmployeeId { get; set; }

    public static UserDTO Create(ApplicationUser user)
        => new()
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PeselNumber = user.PeselNumber,
            PhoneNumber = user.PhoneNumber!,
            DateOfBirth = user.DateOfBirth.ToDateTime(new TimeOnly()),
            EmployeeId = user.EmployeeId
        };

    public static UserDetailsDTO CreateWithDetails(ApplicationUser user, IEnumerable<string> userRoles)
        => new()
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}",
            UserName = user.UserName,
            PeselNumber = user.PeselNumber,
            PhoneNumber = user.PhoneNumber!,
            DateOfBirth = user.DateOfBirth.ToDateTime(new TimeOnly()),
            EmployeeId = user.EmployeeId,
            IsEmailConfirmed = user.EmailConfirmed,
            IsLockout = user.LockoutEnd is not null,
            LockoutEnd = user.LockoutEnd is null
                ? null
                : user.LockoutEnd.Value.UtcDateTime,
            AccountStatus = user.LockoutEnd is not null
                ? "Zablokowane"
                : "Aktywne",
            Roles = userRoles.ToList()
        };

    public static void Update(ApplicationUser user, UpdateUserRequest request)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth);
        user.PeselNumber = request.PeselNumber;
        user.PhoneNumber = request.PhoneNumber;
    }
}
