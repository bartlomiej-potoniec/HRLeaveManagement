using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class LeaveAllocationConfiguration : IEntityTypeConfiguration<LeaveAllocation>
{
    public void Configure(EntityTypeBuilder<LeaveAllocation> builder)
    {
        builder
            .HasOne(la => la.Employee)
            .WithMany()
            .HasForeignKey(la => la.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(la => la.LeaveType)
            .WithMany()
            .HasForeignKey(la => la.LeaveTypeId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.ToTable(la => 
        {
            la.HasCheckConstraint("CK_LeaveAllocation_Year_GreaterThanOrEqualToCurrent", "[Year] >= YEAR(GETDATE())");
            la.HasCheckConstraint("CK_LeaveAllocation_AvailableDays_GreaterThanOrEqualToZero", "[AvailableDays] IS NULL OR [AvailableDays] >= 0");
            la.HasCheckConstraint("CK_LeaveAllocation_UsedDays_FewerThanOrEqualToAvailableDays", "[UsedDays] IS NULL OR [UsedDays] <= [AvailableDays]");
            la.HasCheckConstraint("CK_LeaveAllocation_RemainingDays_FewerThanOrEqualToAvailableDays", "[RemainingDays] IS NULL OR [RemainingDays] <= [AvailableDays]");
        });
    }
}
