using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class TimeRegisterConfiguration : IEntityTypeConfiguration<TimeRegister>
{
    public void Configure(EntityTypeBuilder<TimeRegister> builder)
    {
        builder
            .HasOne(tr => tr.Employee)
            .WithMany()
            .HasForeignKey(tr => tr.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
