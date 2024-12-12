using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Infrastructure.Email.Settings;
using HRLeaveManagement.Infrastructure.Email.Services;
using HRLeaveManagement.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRLeaveManagement.Infrastructure.Extensions;

public static class InfrastructureServicesRegistrationExtension
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
                                                                    IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        return services;
    }
}
