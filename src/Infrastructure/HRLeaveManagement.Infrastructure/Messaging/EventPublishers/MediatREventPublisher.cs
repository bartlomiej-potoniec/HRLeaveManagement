using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using MediatR;

namespace HRLeaveManagement.Infrastructure.Messaging.EventPublishers;

public sealed class MediatREventPublisher<TTransportEvent>(IMediator mediator) 
    : IEventPublisher<TTransportEvent>
    where TTransportEvent : INotification
{
    private readonly IMediator _mediator = mediator;

    public Task PublishAsync(object @event, CancellationToken cancellationToken = default)
        => PublishAsync((TTransportEvent)@event, cancellationToken);

    public async Task PublishAsync(TTransportEvent @event, CancellationToken cancellationToken = default)
        => await _mediator.Publish(@event, cancellationToken);
}
