using HRLeaveManagement.Application.DTOs.LeaveTypes;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries;

public sealed record GetAllLeaveTypesQuery : IRequest<IEnumerable<LeaveTypeDTO>>;
