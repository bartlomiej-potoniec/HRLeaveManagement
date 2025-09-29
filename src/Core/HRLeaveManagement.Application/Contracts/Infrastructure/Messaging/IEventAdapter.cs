using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Contracts;

namespace HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;

public interface IEventAdapter
{
    object Map(IEntityEvent? @event, AuthMetadata authMetadata);
}

public interface IEventAdapter<TTransportEvent> : IEventAdapter
{
    new TTransportEvent Map(IEntityEvent? @event, AuthMetadata authMetadata);
}