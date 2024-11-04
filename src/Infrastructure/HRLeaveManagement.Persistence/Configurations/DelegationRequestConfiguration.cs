using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class DelegationRequestConfiguration : IEntityTypeConfiguration<DelegationRequest>
{
    public void Configure(EntityTypeBuilder<DelegationRequest> builder)
    {
        builder
            .Property(dr => dr.DestinationCountry)
            .HasMaxLength(60);

        builder
            .Property(dr => dr.MeansOfTransport)
            .HasMaxLength(30);

        builder
            .Property(dr => dr.PurposeDescription)
            .HasMaxLength(500);


        builder
            .HasOne(dr => dr.Substitutor)
            .WithMany()
            .HasForeignKey(dr => dr.SubstitutorId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.ToTable(dr => 
            dr.HasCheckConstraint("CK_DelegationRequest_CashAdvance_GreaterThanOrEqualToZero", "[CashAdvance] >= 0.0"));
    }
}
