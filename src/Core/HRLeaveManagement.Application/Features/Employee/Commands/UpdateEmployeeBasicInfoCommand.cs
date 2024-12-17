using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeBasicInfoCommand(Guid Id,
                                                    string Position,
                                                    string Responsibilities,
                                                    int? SectionId,
                                                    Guid? LeaderId)
    : IRequest;