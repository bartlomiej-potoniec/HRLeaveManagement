using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRLeaveManagement.Application.Extensions;

public static class ApplicationServiceRegistrationExtension
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        return services;
    }

}
