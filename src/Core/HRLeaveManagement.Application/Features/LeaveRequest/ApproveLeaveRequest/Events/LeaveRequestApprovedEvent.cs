using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.ApproveLeaveRequest.Events;

public sealed record LeaveRequestApprovedEvent(LeaveRequestApproved Payload, AuthMetadata AuthMetadata) : INotification;
