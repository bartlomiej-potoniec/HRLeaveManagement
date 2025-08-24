using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence.DbContexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                         IOutboxMetadataProvider metadataProvider) 
    : DbContext(options)
{
    private readonly IOutboxMetadataProvider _metadataProvider = metadataProvider;

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeContract> EmployeeContracts { get; set; }
    public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
    public DbSet<EmployeeExperience> EmployeeExperiences { get; set; }
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; } // new

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
    public DbSet<OutboxMessage> OutboxMessages { get; set; } // new

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.Events.Any())
            .Select(entry => entry.Entity)
            .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            var metadata = _metadataProvider.GetMetadata();

            var messages = OutboxMessage.CreateForEntity(entity, metadata);
            OutboxMessages.AddRange(messages);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
