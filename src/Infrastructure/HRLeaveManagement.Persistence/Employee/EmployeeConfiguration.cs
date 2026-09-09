using EmployeeEntity = HRLeaveManagement.Domain.Employee.Employee;
using SectionEntity = HRLeaveManagement.Domain.Department.Section.Section;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Employee;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder
            .Property(e => e.Position)
            .HasMaxLength(50);

        builder
            .Property(e => e.Responsibilities)
            .HasMaxLength(200);

        builder
            .HasOne<SectionEntity>()
            .WithMany()
            .HasForeignKey(e => e.SectionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(e => e.Leader)
            .WithMany()
            .HasForeignKey(e => e.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
