using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeWithDetailsCommand(Guid Id,
                                                      string Position,
                                                      string Responsibilities,
                                                      int? SectionId,
                                                      Guid? LeaderId,
                                                      List<EmployeeContractDetailsRequest> EmployeeContracts,
                                                      List<EmployeeEducationDetailsRequest> EmployeeEducations,
                                                      List<EmployeeExperienceDetailsRequest> EmployeeExperiences) 
    : IRequest;
