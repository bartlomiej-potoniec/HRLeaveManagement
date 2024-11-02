using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .Property(e => e.Position)
            .HasMaxLength(50);

        builder
            .Property(e => e.Responsibilities)
            .HasMaxLength(200);


        builder
            .HasOne(e => e.Section)
            .WithOne()
            .HasForeignKey<Employee>(e => e.SectionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(e => e.Leader)
            .WithOne()
            .HasForeignKey<Employee>(e => e.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
