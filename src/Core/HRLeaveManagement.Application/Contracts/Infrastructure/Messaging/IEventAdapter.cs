using HRLeaveManagement.Domain.Contracts;

namespace HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

public interface IEventAdapter
{
    object Map(IEntityEvent? @event);
}

public interface IEventAdapter<TTransportEvent> : IEventAdapter
{
    new TTransportEvent Map(IEntityEvent? @event);
}