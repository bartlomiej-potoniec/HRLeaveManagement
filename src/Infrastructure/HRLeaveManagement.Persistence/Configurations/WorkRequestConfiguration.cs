using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class WorkRequestConfiguration : IEntityTypeConfiguration<WorkRequest>
{
    public void Configure(EntityTypeBuilder<WorkRequest> builder)
    {
        builder
            .Property(wr => wr.ApproverComment)
            .HasMaxLength(500);

        builder
            .HasOne(wr => wr.RequestingEmployee)
            .WithMany()
            .HasForeignKey(wr => wr.RequestingEmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(wr => wr.Approver)
            .WithMany()
            .HasForeignKey(wr => wr.ApproverId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.ToTable(wr =>
        {
            wr.HasCheckConstraint("CK_WorkRequest_StartedAt_GreaterThanOrEqualToToday", "[StartedAt] >= CAST(GETDATE() AS date)");
            wr.HasCheckConstraint("CK_WorkRequest_EndedAt_GreaterThanOrEqualToStartedAt", "[EndedAt] >= [StartedAt]");
            wr.HasCheckConstraint("CK_WorkRequest_TotalDays_GreaterThanOrEqualToZero", "[TotalDays] >= 0");
        });

        builder.UseTptMappingStrategy();
    }
}
