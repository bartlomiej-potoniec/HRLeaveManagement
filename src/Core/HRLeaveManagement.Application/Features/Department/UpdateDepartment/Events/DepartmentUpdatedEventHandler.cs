using MediatR;

namespace HRLeaveManagement.Application.Features.Department.UpdateDepartment.Events;

public sealed class DepartmentUpdatedEventHandler : INotificationHandler<DepartmentUpdatedEvent>
{
    public async Task Handle(DepartmentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
