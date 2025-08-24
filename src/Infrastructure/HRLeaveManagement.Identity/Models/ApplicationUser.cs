using HRLeaveManagement.Application.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; } // new
    public string? PeselNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public Guid? EmployeeId { get; set; }


    #region Identity_Factory_Methods

    public static ApplicationUser Create(string firstName,
                                         string lastName,
                                         string gender,
                                         string? peselNumber,
                                         string phoneNumber,
                                         DateOnly dateOfBirth,
                                         string email,
                                         string userName,
                                         bool isEmailConfirmed = false)
        => new()
        {
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            PeselNumber = peselNumber,
            PhoneNumber = phoneNumber,
            DateOfBirth = dateOfBirth,
            Email = email,
            UserName = userName,
            EmailConfirmed = isEmailConfirmed
        };

    public static ApplicationUser Create(string firstName,
                                         string lastName,
                                         string gender,
                                         string? peselNumber,
                                         string phoneNumber,
                                         DateTime dateOfBirth,
                                         string email,
                                         string userName,
                                         bool isEmailConfirmed = false)
        => new()
        {
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            PeselNumber = peselNumber,
            PhoneNumber = phoneNumber,
            DateOfBirth = DateOnly.FromDateTime(dateOfBirth),
            Email = email,
            UserName = userName,
            EmailConfirmed = isEmailConfirmed
        };

    public static UserDTO CreateUserDTO(ApplicationUser user)
        => new()
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            PeselNumber = user.PeselNumber,
            PhoneNumber = user.PhoneNumber!,
            DateOfBirth = user.DateOfBirth.ToDateTime(new TimeOnly()),
            EmployeeId = user.EmployeeId
        };

    public static UserDetailsDTO CreateUserDetailsDTO(ApplicationUser user, IEnumerable<string> userRoles)
        => new()
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}",
            Gender = user.Gender,
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
        user.Gender = request.Gender;
        user.Email = request.Email;
        user.DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth);
        user.PeselNumber = request.PeselNumber;
        user.PhoneNumber = request.PhoneNumber;
    }

    public static void UpdateUserEmployeeId(ApplicationUser user, Guid employeeId)
        => user.EmployeeId = employeeId;

    public static void LockoutUserUntilDateTime(ApplicationUser user, DateTime lockoutEnd)
        => user.LockoutEnd = lockoutEnd;

    public static void UnlockUser(ApplicationUser user) => user.LockoutEnd = null;

    #endregion
}
