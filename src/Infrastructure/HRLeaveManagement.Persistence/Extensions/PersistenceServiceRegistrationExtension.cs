using HRLeaveManagement.Persistence.DbContexts;
using HRLeaveManagement.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();

        return services;
    }
}
