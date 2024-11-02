using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder
            .Property(lt => lt.Name)
            .HasMaxLength(50);

        builder
            .Property(lt => lt.Description)
            .HasMaxLength(200);

        builder
            .Property(lt => lt.PaidFraction)
            .HasPrecision(3, 2);


        builder.ToTable(lt => 
            lt.HasCheckConstraint("LeaveType_PaidFraction_BetweenZeroAndOne", "[PaidFraction] BETWEEN 0.0 AND 1.0"));
    }
}
