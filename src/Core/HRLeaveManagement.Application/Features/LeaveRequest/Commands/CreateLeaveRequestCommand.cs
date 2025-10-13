using HRLeaveManagement.Application.DTOs.Employees;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands;

public sealed record CreateLeaveRequestCommand(int LeaveTypeId,
                                               DateTime StartedAt,
                                               DateTime EndedAt,
                                               Guid ApproverId,
                                               Guid SubstitutorId,
                                               string? RequesterComment,
                                               string? ReasonDescription,
                                               IEnumerable<EmployeeDocumentRequest>? EmployeeDocuments) 
    : IRequest<int>;
