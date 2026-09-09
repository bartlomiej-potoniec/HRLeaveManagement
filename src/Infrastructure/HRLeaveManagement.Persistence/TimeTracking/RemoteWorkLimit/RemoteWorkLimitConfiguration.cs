using HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.TimeTracking.RemoteWorkLimit;

public class RemoteWorkLimitConfiguration : IEntityTypeConfiguration<RemoteWorkLimit>
{
    public void Configure(EntityTypeBuilder<RemoteWorkLimit> builder)
    {
        builder
            .HasOne(rwl => rwl.Employee)
            .WithMany()
            .HasForeignKey(rwl => rwl.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.ToTable(rwl =>
        {
            rwl.HasCheckConstraint("CK_RemoteWorkLimit_Year_GreaterThanOrEqualToCurrent", "[Year] >= YEAR(GETDATE())");
            rwl.HasCheckConstraint("CK_RemoteWorkLimit_AvailableDays_GreaterThanOrEqualToZero", "[AvailableDays] >= 0");
            rwl.HasCheckConstraint("CK_RemoteWorkLimit_UsedDays_FewerThanOrEqualToAvailableDays", "[UsedDays] <= [AvailableDays]");
            rwl.HasCheckConstraint("CK_RemoteWorkLimit_RemainingDays_FewerThanOrEqualToAvailableDays", "[RemainingDays] <= [AvailableDays]");
        });
    }
}
