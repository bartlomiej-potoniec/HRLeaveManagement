using HRLeaveManagement.Domain.WorkRequest.ExtraRemoteWorkRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.WorkRequest.ExtraRemoteWorkRequest;

public class ExtraRemoteWorkRequestConfiguration : IEntityTypeConfiguration<ExtraRemoteWorkRequest>
{
    public void Configure(EntityTypeBuilder<ExtraRemoteWorkRequest> builder)
    {
        builder
            .Property(rwr => rwr.ReasonDescription)
            .HasMaxLength(500);
    }
}
