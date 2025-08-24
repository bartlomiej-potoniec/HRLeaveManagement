using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Events;

public sealed record LeaveRequestCreatedEvent(LeaveRequestCreated Payload)
    : INotification;
