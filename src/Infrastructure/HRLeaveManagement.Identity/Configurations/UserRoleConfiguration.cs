using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Identity.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    { 
        builder.HasData(new IdentityUserRole<string> 
        { 
            RoleId = "9e69b4ba-d2df-461e-80d4-d704c73cb550", 
            UserId = "ed62efd7-a3da-49de-b702-c422ec7b0165" 
        });
    }
}
