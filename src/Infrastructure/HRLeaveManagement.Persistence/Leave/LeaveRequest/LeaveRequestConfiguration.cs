using HRLeaveManagement.Domain.Leave.LeaveRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Leave.LeaveRequest;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder
            .Property(lr => lr.Comment)
            .HasMaxLength(500);

        builder
            .Property(lr => lr.ApproverComment)
            .HasMaxLength(500);

        builder
            .Property(lr => lr.ReasonDescription)
            .HasMaxLength(500);

        builder
            .Property(lr => lr.DocumentPath)
            .HasMaxLength(260);


        builder
            .HasOne(lr => lr.RequestingEmployee)
            .WithMany()
            .HasForeignKey(lr => lr.RequestingEmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lr => lr.LeaveType)
            .WithMany()
            .HasForeignKey(lr => lr.LeaveTypeId) 
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lr => lr.Substitutor)
            .WithMany()
            .HasForeignKey(lr => lr.SubstitutorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(lr => lr.Approver)
            .WithMany()
            .HasForeignKey(lr => lr.ApproverId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.ToTable(lr =>
        {
            lr.HasCheckConstraint("CK_LeaveRequest_StartedAt_GreaterThanOrEqualToToday", "[StartedAt] >= CAST(GETDATE() AS date)");
            lr.HasCheckConstraint("CK_LeaveRequest_EndedAt_GreaterThanOrEqualToStartedAt", "[EndedAt] >= StartedAt");
            lr.HasCheckConstraint("CK_LeaveRequest_TotalDays_GreaterThanZero", "[TotalDays] > 0");
        });
    }
}
