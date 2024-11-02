using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Configurations;

public class EmployeeExperienceConfiguration : IEntityTypeConfiguration<EmployeeExperience>
{
    public void Configure(EntityTypeBuilder<EmployeeExperience> builder)
    {
        builder
            .HasOne(ee => ee.Employee)
            .WithMany()
            .HasForeignKey(ee => ee.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.ToTable(ee => 
            ee.HasCheckConstraint("CK_EmployeeExperience_TotalEmployment_GreaterThanZero", "[TotalEmployment] > 0"));
    }
}
