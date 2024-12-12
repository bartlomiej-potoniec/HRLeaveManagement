using HRLeaveManagement.Identity.Models;

namespace HRLeaveManagement.Identity.Tests.Mocks;

public static class ApplicationUserMock
{
    public static ApplicationUser Create(string userName = "jkowals95",
                                         string email = "jkowalski95@company.com",
                                         string passwordHash = "password_hash_for_P@ssword1",
                                         string firstName = "Jan",
                                         string lastName = "Kowalski",
                                         DateOnly? dateOfBirth = null,
                                         bool emailConfirmed = false)
        => new()
        {
            Id = "512faae2-6d86-4286-ab16-903f68582497",
            UserName = userName,
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = (DateOnly)(dateOfBirth is null ? new(1995, 4, 12) : dateOfBirth),
            EmailConfirmed = emailConfirmed
        };
}
