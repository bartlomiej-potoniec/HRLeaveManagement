using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Events;

public sealed record LeaveRequestRejectedEvent(LeaveRequestRejected Payload, AuthMetadata AuthMetadata) : INotification;
