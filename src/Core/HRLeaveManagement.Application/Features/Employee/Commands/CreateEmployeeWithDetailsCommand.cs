using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record CreateEmployeeWithDetailsCommand(Guid UserId,
                                                      string Position,
                                                      string Responsibilities,
                                                      int? SectionId,
                                                      Guid? LeaderId,
                                                      EmployeeContractRequest EmployeeContract,
                                                      List<EmployeeEducationRequest> EmployeeEducations,
                                                      List<EmployeeExperienceRequest> EmployeeExperiences)
    : IRequest<Guid>;
