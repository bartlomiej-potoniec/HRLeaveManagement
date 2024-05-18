using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries;

public sealed record GetLeaveTypeDetailsQuery(int Id) : IRequest<LeaveTypeDetailsDTO>;
