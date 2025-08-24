using HRLeaveManagement.Domain.RuleContracts;
using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Persistence.Repositories;
using HRLeaveManagement.Persistence.RuleSets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence;

namespace HRLeaveManagement.Persistence.Extensions;

public static class PersistenceServiceRegistrationExtension
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services,
                                                                 IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration
                .GetConnectionString("HrLeaveManagementConnectionString"))
        );

        services.AddScoped<IEmployeeDocumentRuleSet, EmployeeDocumentRuleSet>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRemoteWorkLimitRepository, RemoteWorkLimitRepository>();

        return services;
    }
}
