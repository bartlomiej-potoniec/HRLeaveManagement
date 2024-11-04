using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class SectionConfigutation : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder
            .Property(s => s.Name)
            .HasMaxLength(50);

        builder
            .Property(s => s.Description)
            .HasMaxLength(100);


        builder
            .HasOne(s => s.Department)
            .WithMany(d => d.Sections)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
