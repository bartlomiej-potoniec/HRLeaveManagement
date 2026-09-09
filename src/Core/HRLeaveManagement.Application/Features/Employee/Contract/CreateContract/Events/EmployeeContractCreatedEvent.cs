using HRLeaveManagement.Application.DTOs.Auth;
using HRLeaveManagement.Domain.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.CreateContract.Events;

public sealed record EmployeeContractCreatedEvent(EmployeeContractCreated Payload, AuthMetadata AuthMetadata)
    : INotification;
