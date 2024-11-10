using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50);
        
        builder
            .Property(u => u.LastName)
            .HasMaxLength(50);

        builder
            .Property(u => u.PeselNumber)
            .HasMaxLength(11);
        

        builder.HasData(new ApplicationUser
        {
            Id = "ed62efd7-a3da-49de-b702-c422ec7b0165",
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            FirstName = "System",
            LastName = "Admin",
            DateOfBirth = new(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            PasswordHash = HashPassword(null, "admin"),
            EmailConfirmed = true
        });
    }

    private static string HashPassword(ApplicationUser user, string password)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var passwordHash = hasher.HashPassword(user, password);

        return passwordHash;
    }
}