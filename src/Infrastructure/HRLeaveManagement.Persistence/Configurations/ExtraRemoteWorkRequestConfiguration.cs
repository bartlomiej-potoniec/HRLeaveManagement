using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class ExtraRemoteWorkRequestConfiguration : IEntityTypeConfiguration<ExtraRemoteWorkRequest>
{
    public void Configure(EntityTypeBuilder<ExtraRemoteWorkRequest> builder)
    {
        builder
            .Property(rwr => rwr.ReasonDescription)
            .HasMaxLength(500);


        builder.ToTable("ExtraRemoteWorkRequest");
    }
}
