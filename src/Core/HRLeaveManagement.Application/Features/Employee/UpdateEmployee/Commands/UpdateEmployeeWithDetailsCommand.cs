using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Commands;

public sealed record UpdateEmployeeWithDetailsCommand(Guid Id,
                                                      string Position,
                                                      string Responsibilities,
                                                      string ResidentialAddress, // new
                                                      string RegisteredAddress, // new
                                                      string? SecondaryResidentialAddress, // new
                                                      string? RemoteWorkAddress, // new
                                                      int? SectionId,
                                                      Guid? LeaderId,
                                                      List<EmployeeContractDetailsRequest> EmployeeContracts,
                                                      List<EmployeeEducationDetailsRequest> EmployeeEducations,
                                                      List<EmployeeExperienceDetailsRequest> EmployeeExperiences) 
    : IRequest;
