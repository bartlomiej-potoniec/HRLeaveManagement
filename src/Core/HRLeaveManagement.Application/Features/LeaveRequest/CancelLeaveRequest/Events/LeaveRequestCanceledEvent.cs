using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CancelLeaveRequest.Events;

public sealed record LeaveRequestCanceledEvent(LeaveRequestCanceled Payload, AuthMetadata AuthMetadata)
    : INotification;
