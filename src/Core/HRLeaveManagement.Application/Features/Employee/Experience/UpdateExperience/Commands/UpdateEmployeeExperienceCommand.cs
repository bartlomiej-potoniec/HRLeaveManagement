using HRLeaveManagement.Domain.Employee.Contract;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Experience.UpdateExperience.Commands;

public sealed record UpdateEmployeeExperienceCommand(Guid EmployeeId,
                                                     int ExperienceId,
                                                     ContractType ContractType,
                                                     string PreviousCompanyName,
                                                     string Position,
                                                     DateTime EmployedFrom,
                                                     DateTime EmployedTo) 
    : IRequest;