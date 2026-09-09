using HRLeaveManagement.Domain.WorkRequest.OvertimeRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.WorkRequest.OvertimeRequest;

public class OvertimeRequestConfiguration : IEntityTypeConfiguration<OvertimeRequest>
{
    public void Configure(EntityTypeBuilder<OvertimeRequest> builder)
    {
        builder
            .Property(or => or.PurposeDescription)
            .HasMaxLength(500);
    }
}
