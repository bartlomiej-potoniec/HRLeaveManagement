using HRLeaveManagement.Domain.Employee.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRLeaveManagement.Persistence.Employee.Contract;

public class EmployeeContractConfiguration : IEntityTypeConfiguration<EmployeeContract>
{
    public void Configure(EntityTypeBuilder<EmployeeContract> builder)
    {
        builder
            .HasOne(ec => ec.Employee)
            .WithMany(e => e.EmployeeContracts)
            .HasForeignKey(ec => ec.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.ToTable(ec =>
            ec.HasCheckConstraint("CK_EmployeeContract_TotalDuration_GreaterThanZero", "[TotalDuration] IS NULL OR [TotalDuration] > 0"));
    }
}
