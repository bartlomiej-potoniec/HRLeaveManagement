using HRLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.DbContexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeContract> EmployeeContracts { get; set; }
    public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
    public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }

    public DbSet<Section> Sections { get; set; }
    public DbSet<Department> Departments { get; set; }

    public DbSet<TimeRegister> TimeRegisters { get; set; }

    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public DbSet<WorkRequest> WorkRequests { get; set; }
    public DbSet<ExtraRemoteWorkRequest> ExtraRemoteWorkRequests { get; set; }
    public DbSet<OvertimeRequest> OvertimeRequests { get; set; }
    public DbSet<DelegationRequest> DelegationRequests {  get; set; }   

    public DbSet<RemoteWorkLimit> RemoteWorkLimits { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
