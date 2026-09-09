using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Events;

public sealed record EmployeeUpdatedEvent(EmployeeUpdated Payload, AuthMetadata AuthMetadata)
    : INotification;
