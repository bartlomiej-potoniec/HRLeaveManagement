using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Events;

public sealed record EmployeeCreatedEvent(EmployeeCreated Payload) : INotification;
