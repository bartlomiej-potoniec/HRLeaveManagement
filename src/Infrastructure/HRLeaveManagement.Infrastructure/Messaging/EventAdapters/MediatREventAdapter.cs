using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Application.Contracts.Infrastructure.Messaging;
using HRLeaveManagement.Application.Features.LeaveRequest.Events;
using HRLeaveManagement.Application.Features.Employee.Events;
using HRLeaveManagement.Application.Features.Department.Events;
using MediatR;

namespace HRLeaveManagement.Infrastructure.Messaging.EventAdapters;

public sealed class MediatREventAdapter<TTransportEvent> : IEventAdapter<TTransportEvent>
    where TTransportEvent : INotification
{
    object IEventAdapter.Map(IEntityEvent? @event, AuthMetadata authMetadata) => Map(@event, authMetadata);
    public TTransportEvent Map(IEntityEvent? @event, AuthMetadata authMetadata) 
        => (TTransportEvent)(object)(GetAvailableEvents(@event, authMetadata));

    private static INotification GetAvailableEvents(IEntityEvent? @event, AuthMetadata authMetadata)
        => @event switch
        {
            EmployeeCreated ev      => new EmployeeCreatedEvent(ev, authMetadata),
            DepartmentCreated ev    => new DepartmentCreatedEvent(ev, authMetadata),
            DepartmentUpdated ev    => new DepartmentUpdatedEvent(ev, authMetadata),
            LeaveRequestCreated ev  => new LeaveRequestCreatedEvent(ev, authMetadata),
            LeaveRequestCanceled ev => new LeaveRequestCanceledEvent(ev, authMetadata),
            _                       => throw new NotImplementedException("There's no notification-event with given type")
        };
}
