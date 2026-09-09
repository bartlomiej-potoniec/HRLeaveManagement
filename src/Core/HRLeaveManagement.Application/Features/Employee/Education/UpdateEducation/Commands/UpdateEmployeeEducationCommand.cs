using HRLeaveManagement.Domain.Employee.Education;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Education.UpdateEducation.Commands;

public sealed record UpdateEmployeeEducationCommand(Guid EmployeeId,
                                                    int EducationId,
                                                    EducationType EducationType,
                                                    string EducationDetails,
                                                    DateTime EnrolledAt,
                                                    DateTime? GraduatedAt) 
    : IRequest;
