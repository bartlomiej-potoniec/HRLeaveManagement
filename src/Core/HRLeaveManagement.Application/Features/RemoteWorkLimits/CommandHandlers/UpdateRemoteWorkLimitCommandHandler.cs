using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.RemoteWorkLimits.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimits.CommandHandlers;

public sealed class UpdateRemoteWorkLimitCommandHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                        IEmployeeRepository employeeRepository,
                                                        IAppLogger<UpdateRemoteWorkLimitCommandHandler> logger) 
    : IRequestHandler<UpdateRemoteWorkLimitCommand>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IAppLogger<UpdateRemoteWorkLimitCommandHandler> _logger = logger;

    public async Task Handle(UpdateRemoteWorkLimitCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new UpdateRemoteWorkLimitCommandValidator(
            _employeeRepository,
            _remoteWorkLimitRepository
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(UpdateRemoteWorkLimitCommand));
            throw new BadRequestException("Invalid department updating request", validationResult);
        }

        var remoteWorkLimit = await _remoteWorkLimitRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No remote work limit with ID: { request.Id } found");

        RemoteWorkLimit.Update(
            remoteWorkLimit,
            request.EmployeeId,
            request.Year,
            request.AvailableDays
        );

        _logger.LogInformation("Updating informations about remote work limit with ID: {Id} started", request.Id);

        await _remoteWorkLimitRepository.Update(remoteWorkLimit);

        _logger.LogInformation("Updating informations about remote work limit with ID: {Id} successful", request.Id);
    }
}
