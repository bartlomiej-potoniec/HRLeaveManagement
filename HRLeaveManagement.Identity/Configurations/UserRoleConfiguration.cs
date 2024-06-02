using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        var seedData = new IdentityUserRole<string>[]
        {
            new() { RoleId = "ae1e9edb-d020-4fd4-8c39-eb6557aee3c2", UserId = "9a9a289c-56e5-4dd6-917e-d892b381f815" },
            new() { RoleId = "9e69b4ba-d2df-461e-80d4-d704c73cb550", UserId = "ed62efd7-a3da-49de-b702-c422ec7b0165" }
        };

        builder.HasData(seedData);
    }
}
