using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Education.CreateEducation.Commands;

public sealed record CreateEmployeeEducationCommand(Guid EmployeeId)
    : IRequest<Guid>;