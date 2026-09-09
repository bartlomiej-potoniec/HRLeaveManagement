using HRLeaveManagement.Domain.TimeTracking.TimeRegister;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.TimeTracking.TimeRegister;

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
