using HRLeaveManagement.Application.DTOs;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries;

public record GetAllLeaveTypesQuery : IRequest<IEnumerable<LeaveTypeDTO>>;
