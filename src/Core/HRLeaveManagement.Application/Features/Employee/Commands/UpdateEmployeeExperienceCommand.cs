using HRLeaveManagement.Domain.Enums;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeExperienceCommand(Guid EmployeeId,
                                                     int ExperienceId,
                                                     ContractType ContractType,
                                                     string PreviousCompanyName,
                                                     string Position,
                                                     DateTime EmployedFrom,
                                                     DateTime EmployedTo) 
    : IRequest;