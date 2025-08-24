using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using HRLeaveManagement.Application.Features.Employee.Events;
using MediatR;

namespace HRLeaveManagement.Infrastructure.Messaging.EventAdapters;

public sealed class MediatREventAdapter<TTransportEvent> : IEventAdapter<TTransportEvent>
    where TTransportEvent : INotification
{
    object IEventAdapter.Map(IEntityEvent? @event) => Map(@event);
    public TTransportEvent Map(IEntityEvent? @event) => (TTransportEvent)(object)(GetAvailableEvents(@event));

    private static INotification GetAvailableEvents(IEntityEvent? @event)
        => @event switch
        {
            LeaveRequestCreated ev => new LeaveRequestCreatedEvent(ev),
            EmployeeCreated ev     => new EmployeeCreatedEvent(ev),
            _                      => throw new NotImplementedException("There's no notification-event with given type")
        };
}
