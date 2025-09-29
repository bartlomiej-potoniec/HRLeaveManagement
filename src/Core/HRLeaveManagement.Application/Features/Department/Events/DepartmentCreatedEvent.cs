using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Application.DTOs.Auth;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.Events;

public sealed record DepartmentCreatedEvent(DepartmentCreated Payload, AuthMetadata AuthMetadata) : INotification;
