using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.CommandHandlers;

public sealed class UpdateRemoteWorkLimitCommandHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                        IEmployeeRepository employeeRepository,
                                                        TimeProvider timeProvider,
                                                        IAppLogger<UpdateRemoteWorkLimitCommandHandler> logger) 
    : IRequestHandler<UpdateRemoteWorkLimitCommand>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<UpdateRemoteWorkLimitCommandHandler> _logger = logger;

    public async Task Handle(UpdateRemoteWorkLimitCommand request, CancellationToken cancellationToken)
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
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No remote work limit with ID: { request.Id } found");

        var currentYear = _timeProvider.GetUtcNow().Year;

        remoteWorkLimit.Update(currentYear, request.Year, request.AvailableDays);

        _logger.LogInformation("Updating informations about remote work limit with ID: {Id} started", request.Id);

        await _remoteWorkLimitRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updating informations about remote work limit with ID: {Id} successful", request.Id);
    }
}
