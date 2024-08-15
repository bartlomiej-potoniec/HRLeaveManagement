using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries;

public sealed record GetAllLeaveRequestsWithDetailsQuery(bool IsUserLoggedIn) 
    : IRequest<IEnumerable<LeaveRequestDTO>>;
