using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Employee.Events;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.EventHandlers;

public sealed class EmployeeContractCreatedEventHandler(IEmailService emailService,
                                                        IAppLogger<EmployeeContractCreatedEventHandler> logger)
    : INotificationHandler<EmployeeContractCreatedEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<EmployeeContractCreatedEventHandler> _logger = logger;

    public async Task Handle(EmployeeContractCreatedEvent notification, CancellationToken cancellationToken)
    {
        var requestingUserEmail = notification.AuthMetadata.RequestingUserEmail;
        var contractType = notification.Payload.ContractType;
        var employeeName = notification.Payload.EmployeeFullName;

        _logger.LogInformation("Sending contract-creation email to user with email: {UserEmail}", requestingUserEmail);
        await _emailService.SendEmployeeContractCreationEmailAsync(
            requestingUserEmail,
            contractType,
            employeeName,
            cancellationToken
        );
    }
}
