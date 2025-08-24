using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Application.Contracts.Infrastructure.Email;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Infrastructure.Messaging;
using HRLeaveManagement.Infrastructure.Messaging.EventAdapters;
using HRLeaveManagement.Infrastructure.Messaging.EventPublishers;
using HRLeaveManagement.Infrastructure.Messaging.Options;
using HRLeaveManagement.Infrastructure.Email.Services;
using HRLeaveManagement.Infrastructure.Email.Options;
using HRLeaveManagement.Infrastructure.Logging;
using HRLeaveManagement.Infrastructure.Sieve;
using HRLeaveManagement.Infrastructure.Sieve.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sieve.Models;
using Sieve.Services;
using MediatR;

namespace HRLeaveManagement.Infrastructure.Extensions;

public static class InfrastructureServicesRegistrationExtension
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
                                                                    IConfiguration configuration)
    {
        services.AddHostedService<OutboxMessageProcessor>();

        services.AddTransient(typeof(IEventPublisher<>), typeof(MediatREventPublisher<>));
        services.AddTransient<IEventPublisher>(provider =>
            provider.GetRequiredService<IEventPublisher<INotification>>());

        services.AddTransient(typeof(IEventAdapter<>), typeof(MediatREventAdapter<>));
        services.AddTransient<IEventAdapter>(provider =>
            provider.GetRequiredService<IEventAdapter<INotification>>());

        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
        services.Configure<SieveOptions>(configuration.GetSection("Sieve"));

        services.AddTransient<IEmailSender, EmailSender>();

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomRolesFilterMethods>();

        return services;
    }
}
