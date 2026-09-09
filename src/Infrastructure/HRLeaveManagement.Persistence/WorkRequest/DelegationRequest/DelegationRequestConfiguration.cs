using DomainDelegationRequest = HRLeaveManagement.Domain.WorkRequest.DelegationRequest.DelegationRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.WorkRequest.DelegationRequest;

public class DelegationRequestConfiguration : IEntityTypeConfiguration<DomainDelegationRequest>
{
    public void Configure(EntityTypeBuilder<DomainDelegationRequest> builder)
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
