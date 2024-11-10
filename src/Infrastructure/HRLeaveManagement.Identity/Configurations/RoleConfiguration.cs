using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        IdentityRole[] seedData = [
            new() { Id = "ae1e9edb-d020-4fd4-8c39-eb6557aee3c2", Name = "Employee", NormalizedName = "EMPLOYEE" },
            new() { Id = "5746cb95-1b05-4231-8e87-3648e4fb9208", Name = "HR", NormalizedName = "HR" },
            new() { Id = "19f6a619-c49c-4a46-87f7-cd58478af056", Name = "Manager", NormalizedName = "MANAGER" },
            new() { Id = "83476db7-e703-4b28-9c17-7ee12a2ddab1", Name = "CEO", NormalizedName = "CEO" },
            new() { Id = "9e69b4ba-d2df-461e-80d4-d704c73cb550", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
        ];

        builder.HasData(seedData);
    }
}