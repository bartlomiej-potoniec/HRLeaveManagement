using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Identity.DbContexts;

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationIdentityDbContext).Assembly);
    }
}
