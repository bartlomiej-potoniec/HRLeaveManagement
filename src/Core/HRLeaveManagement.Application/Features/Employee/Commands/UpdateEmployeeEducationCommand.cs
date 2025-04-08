using HRLeaveManagement.Domain.Enums;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeEducationCommand(Guid EmployeeId,
                                                    int EducationId,
                                                    EducationType EducationType,
                                                    string EducationDetails,
                                                    DateTime EnrolledAt,
                                                    DateTime? GraduatedAt) 
    : IRequest;
