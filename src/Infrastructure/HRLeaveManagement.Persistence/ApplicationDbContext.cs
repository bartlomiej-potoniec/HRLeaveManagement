using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;
using HRLeaveManagement.Domain.Leave.LeaveAllocation;
using HRLeaveManagement.Domain.Leave.LeaveRequest;
using HRLeaveManagement.Domain.Leave.LeaveType;
using HRLeaveManagement.Domain.Department;
using HRLeaveManagement.Domain.Department.Section;
using HRLeaveManagement.Domain.Outbox;
using HRLeaveManagement.Domain.TimeTracking.RemoteWorkLimit;
using HRLeaveManagement.Domain.TimeTracking.TimeRegister;
using HRLeaveManagement.Domain.WorkRequest;
using HRLeaveManagement.Domain.WorkRequest.DelegationRequest;
using HRLeaveManagement.Domain.WorkRequest.ExtraRemoteWorkRequest;
using HRLeaveManagement.Domain.WorkRequest.OvertimeRequest;
using Microsoft.EntityFrameworkCore;

namespace HRLeaveManagement.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                         IAuthMetadataProvider metadataProvider) 
    : DbContext(options)
{
    private readonly IAuthMetadataProvider _metadataProvider = metadataProvider;

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

    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    var entitiesWithEvents = ChangeTracker
    //        .Entries<Entity>()
    //        .Where(entry => entry.Entity.Events.Any())
    //        .Select(entry => entry.Entity)
    //        .ToList();

    //    foreach (var entity in entitiesWithEvents)
    //    {
    //        var metadata = _metadataProvider.GetMetadata();
    //        string userId = metadata.RequestingUserId;

    //        var messages = OutboxMessage.CreateForEntity(entity, userId, metadata);
    //        OutboxMessages.AddRange(messages);
    //    }

    //    return await base.SaveChangesAsync(cancellationToken);
    //}

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var existingMessageIds = OutboxMessages
            .Select(message => message.Id)
            .ToHashSet();

        var messages = ChangeTracker
            .Entries<OutboxMessage>()
            .Where(entry => (entry.Entity.IsProcessed && existingMessageIds.Contains(entry.Entity.Id)) is false)
            .Select(entry => entry.Entity);

        await OutboxMessages.AddRangeAsync(messages, cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }
}
