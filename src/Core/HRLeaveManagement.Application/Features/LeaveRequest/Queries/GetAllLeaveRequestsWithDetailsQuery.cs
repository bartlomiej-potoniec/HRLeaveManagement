using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries;

public sealed record GetAllLeaveRequestsWithDetailsQuery(bool IsUserLoggedIn) // bool parameter is intended to be deleted from the param-list
    : IRequest<IEnumerable<LeaveRequestDTO>>;
