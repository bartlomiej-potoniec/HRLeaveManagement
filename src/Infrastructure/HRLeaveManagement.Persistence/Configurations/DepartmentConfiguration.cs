using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder
            .Property(d => d.Name)
            .HasMaxLength(50);

        builder
            .Property(d => d.Description)
            .HasMaxLength(100);


        builder
            .HasOne(d => d.Leader)
            .WithMany()
            .HasForeignKey(d => d.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
