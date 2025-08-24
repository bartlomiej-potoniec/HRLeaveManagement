using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeWithDetailsCommand(Guid UserId,
                                                      string Position,
                                                      string Responsibilities,
                                                      string ResidentialAddress, // new
                                                      string RegisteredAddress, // new
                                                      string? SecondaryResidentialAddress, // new
                                                      string? RemoteWorkAddress, // new
                                                      int? SectionId,
                                                      Guid? LeaderId,
                                                      EmployeeContractRequest EmployeeContract,
                                                      IEnumerable<EmployeeEducationRequest> EmployeeEducations,
                                                      IEnumerable<EmployeeExperienceRequest> EmployeeExperiences)
    : IRequest<Guid>;
