using HRLeaveManagement.Application.DTOs.LeaveTypes;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.GetLeaveType.Queries;

public sealed record GetAllLeaveTypesQuery : IRequest<IEnumerable<LeaveTypeDTO>>;
