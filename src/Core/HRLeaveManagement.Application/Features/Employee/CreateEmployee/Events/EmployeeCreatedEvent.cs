using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CreateEmployee.Events;

public sealed record EmployeeCreatedEvent(EmployeeCreated Payload, AuthMetadata AuthMetadata) : INotification;
