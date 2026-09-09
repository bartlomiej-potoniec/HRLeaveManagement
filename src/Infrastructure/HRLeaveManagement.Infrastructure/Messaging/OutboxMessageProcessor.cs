using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Infrastructure.Messaging.Options;
using HRLeaveManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HRLeaveManagement.Infrastructure.Messaging;

public sealed class OutboxMessageProcessor(IServiceScopeFactory serviceScopeFactory,
                                           IAppLogger<OutboxMessageProcessor> logger,
                                           IOptions<OutboxOptions> outboxOptions)
    : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IAppLogger<OutboxMessageProcessor> _logger = logger;
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
            var eventAdapter = scope.ServiceProvider.GetRequiredService<IEventAdapter>();
            var authMetadataProvider = scope.ServiceProvider.GetRequiredService<IAuthMetadataProvider>();

            var messageCountToProcessing = _outboxOptions.MessageProcessor.MessageCountToProcessing;
            var messageCollectionPeriodInSeconds = _outboxOptions.MessageProcessor.MessageCollectionPeriodInSeconds;

            var messages = await dbContext.OutboxMessages
                .Where(mess => !mess.IsProcessed)
                .OrderBy(mess => mess.OccurredOn)
                .Take(messageCountToProcessing)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    Type? type = Type.GetType(message.Type);
                    if (type is null)
                    {
                        _logger.LogWarning("Unknown message type: {Type}", message.Type);
                        continue;
                    }

                    IEntityEvent? @event = JsonSerializer.Deserialize(message.Payload, type) as IEntityEvent;
                    if (type is null)
                    {
                        _logger.LogWarning("Unknown event type or null");
                        continue;
                    }

                    AuthMetadata? authMetadata = message.AuthMetadata is null 
                        ? null 
                        : JsonSerializer.Deserialize<AuthMetadata>(message.AuthMetadata);

                    if (authMetadata is null)
                    {
                        _logger.LogWarning("Unknown auth-metadata or null");
                        continue;
                    }

                    var transportEvent = eventAdapter.Map(@event, authMetadata);

                    await eventPublisher.PublishAsync(transportEvent, stoppingToken);
                    message.MarkAsProcessed();
                }

                catch (Exception ex)
                {
                    _logger.LogError("Error processing outbox message {Id}:{Message}", message.Id, ex.Message);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);            
            await Task.Delay(messageCollectionPeriodInSeconds, stoppingToken);
        }
    }
}
