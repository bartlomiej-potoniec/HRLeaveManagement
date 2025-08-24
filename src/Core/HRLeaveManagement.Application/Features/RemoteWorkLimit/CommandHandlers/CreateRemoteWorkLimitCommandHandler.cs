using DomainRemoteWorkLimit = HRLeaveManagement.Domain.Entities.RemoteWorkLimit;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.CommandHandlers;

public sealed class CreateRemoteWorkLimitCommandHandler(IRemoteWorkLimitRepository remoteWorkLimitRepository,
                                                        IEmployeeRepository employeeRepository,
                                                        IEmployeeContextFactory employeeContextFactory,
                                                        TimeProvider timeProvider,
                                                        IAppLogger<CreateRemoteWorkLimitCommandHandler> logger)
    : IRequestHandler<CreateRemoteWorkLimitCommand, int>
{
    private readonly IRemoteWorkLimitRepository _remoteWorkLimitRepository = remoteWorkLimitRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly IAppLogger<CreateRemoteWorkLimitCommandHandler> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<int> Handle(CreateRemoteWorkLimitCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRemoteWorkLimitCommandValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid) 
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateRemoteWorkLimitCommand));
            throw new BadRequestException("Invalid remote work limit creation request", validationResult);
        }

        var employee = await _employeeRepository
            .GetWithRemoteWorkLimitsByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.EmployeeId } found");

        var employeeWithRemoteWorkLimits = _employeeContextFactory.AsEmployeeWithRemoteWorkLimits(employee);
        var currentYear = _timeProvider.GetUtcNow().Year;

        var remoteWorkLimit = DomainRemoteWorkLimit.Create(
            employeeWithRemoteWorkLimits,
            currentYear,
            request.Year,
            request.AvailableDays
        );

        employeeWithRemoteWorkLimits.AddRemoteWorkLimit(remoteWorkLimit);

        _logger.LogInformation("Creating new remote work limit for employee with ID: {EmployeeId} started", request.EmployeeId);

        await _employeeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new remote work limit for employee with ID: {EmployeeId} successful", request.EmployeeId);

        return remoteWorkLimit.Id;
    }
}
