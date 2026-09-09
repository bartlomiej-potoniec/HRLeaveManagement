using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CreateLeaveRequest.Events;

public sealed record LeaveRequestCreatedEvent(LeaveRequestCreated Payload, AuthMetadata AuthMetadata)
    : INotification;
