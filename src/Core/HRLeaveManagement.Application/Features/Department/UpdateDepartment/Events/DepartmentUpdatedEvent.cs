using HRLeaveManagement.Domain.Events;
using HRLeaveManagement.Application.DTOs.Auth;
using MediatR;

namespace HRLeaveManagement.Application.Features.Department.UpdateDepartment.Events;

public sealed record DepartmentUpdatedEvent(DepartmentUpdated Payload, AuthMetadata AuthMetadata) : INotification;
