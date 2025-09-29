using HRLeaveManagement.Application.Features.Department.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.EventHandlers;

public sealed class DepartmentUpdatedEventHandler : INotificationHandler<DepartmentUpdatedEvent>
{
    public async Task Handle(DepartmentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
