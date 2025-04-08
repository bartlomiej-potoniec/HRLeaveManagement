using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeEducationCommand(Guid EmployeeId)
    : IRequest<Guid>;