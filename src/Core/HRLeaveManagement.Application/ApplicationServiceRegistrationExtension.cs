using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.Features.LeaveAllocation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRLeaveManagement.Application;

public static class ApplicationServiceRegistrationExtension
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddScoped<IEmployeeSubservice, IEmployeeSubservice>();
        services.AddScoped<LeavePolicyFactory>();

        return services;
    }
}
