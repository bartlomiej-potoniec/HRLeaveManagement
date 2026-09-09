using DepartmentEntity = HRLeaveManagement.Domain.Department.Department;
using EmployeeEntity = HRLeaveManagement.Domain.Employee.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Department;

public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder
            .Property(d => d.Name)
            .HasMaxLength(50);

        builder
            .Property(d => d.Description)
            .HasMaxLength(100);

        builder
            .HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(d => d.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
