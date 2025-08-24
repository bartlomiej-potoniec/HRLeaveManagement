namespace HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(object @event, CancellationToken cancellationToken = default);
}

public interface IEventPublisher<TTransportEvent> : IEventPublisher
{
    Task PublishAsync(TTransportEvent @event, CancellationToken cancellationToken = default);
}
