using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class EmployeeEducationConfiguration : IEntityTypeConfiguration<EmployeeEducation>
{
    public void Configure(EntityTypeBuilder<EmployeeEducation> builder)
    {
        builder
            .Property(ee => ee.EducationDetails)
            .HasMaxLength(200);


        builder
            .HasOne(ee => ee.Employee)
            .WithMany(e => e.EmployeeEducations)
            .HasForeignKey(ee => ee.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
