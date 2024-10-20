using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries;

public sealed record GetLeaveRequestWithDetailsByIdQuery(int Id) : IRequest<LeaveRequestDetailsDTO>;