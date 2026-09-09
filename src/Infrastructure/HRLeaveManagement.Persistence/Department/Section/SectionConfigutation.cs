using SectionEntity = HRLeaveManagement.Domain.Department.Section.Section;
using EmployeeEntity = HRLeaveManagement.Domain.Employee.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Department.Section;

public class SectionConfigutation : IEntityTypeConfiguration<SectionEntity>
{
    public void Configure(EntityTypeBuilder<SectionEntity> builder)
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
        
        builder
            .HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(s => s.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
