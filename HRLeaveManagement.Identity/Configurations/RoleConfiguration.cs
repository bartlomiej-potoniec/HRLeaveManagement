using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        var seedData = new IdentityRole[]
        {
            new() { Id = "ae1e9edb-d020-4fd4-8c39-eb6557aee3c2", Name = "Employee", NormalizedName = "EMPLOYEE" },
            new() { Id = "9e69b4ba-d2df-461e-80d4-d704c73cb550", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
        };

        builder.HasData(seedData);
    }
}
