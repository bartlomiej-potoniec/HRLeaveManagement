using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record UpdateEmployeeBasicInfoCommand(Guid Id,
                                                    string Position,
                                                    string Responsibilities,
                                                    string ResidentialAddress, // new
                                                    string RegisteredAddress, // new
                                                    string? SecondaryResidentialAddress, // new
                                                    string? RemoteWorkAddress, // new
                                                    int? SectionId,
                                                    Guid? LeaderId)
    : IRequest;