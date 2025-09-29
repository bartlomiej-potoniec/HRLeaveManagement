using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Events;

public sealed record LeaveAllocationUpdatedEvent(LeaveAllocationUpdated Payload, AuthMetadata AuthMetadata)
    : INotification;
