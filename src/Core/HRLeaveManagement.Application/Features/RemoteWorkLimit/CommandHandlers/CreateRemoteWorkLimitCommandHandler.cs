using DomainRemoteWorkLimit = HRLeaveManagement.Domain.Entities.RemoteWorkLimit;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.CommandHandlers;

public sealed class CreateRemoteWorkLimitCommandHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                        IEmployeeRepository employeeRepository,
                                                        IAppLogger<CreateRemoteWorkLimitCommandHandler> logger)
    : IRequestHandler<CreateRemoteWorkLimitCommand, int>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IAppLogger<CreateRemoteWorkLimitCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateRemoteWorkLimitCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRemoteWorkLimitCommandValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) 
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateRemoteWorkLimitCommand));
            throw new BadRequestException("Invalid remote work limit creation request", validationResult);
        }

        var remoteWorkLimit = DomainRemoteWorkLimit.Create(
            request.EmployeeId,
            request.Year,
            request.AvailableDays
        );

        _logger.LogInformation("Creating new remote work limit for employee with ID: {EmployeeId} started", request.EmployeeId);

        await _remoteWorkLimitRepository.CreateAsync(remoteWorkLimit, cancellationToken);

        _logger.LogInformation("Creating new remote work limit for employee with ID: {EmployeeId} successful", request.EmployeeId);

        return remoteWorkLimit.Id;
    }
}
