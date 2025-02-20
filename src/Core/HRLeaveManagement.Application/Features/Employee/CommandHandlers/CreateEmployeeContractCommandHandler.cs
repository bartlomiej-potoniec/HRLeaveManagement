using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class CreateEmployeeContractCommandHandler(IEmployeeRepository employeeRepository,
                                                         IMapper mapper,
                                                         IAppLogger<CreateEmployeeContractCommandHandler> logger)
    : IRequestHandler<CreateEmployeeContractCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<CreateEmployeeContractCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateEmployeeContractCommand request,
                                  CancellationToken cancellationToken)
    {
        var validator = new CreateEmployeeContractCommandValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeContractCommand));
            throw new BadRequestException("Invalid employee contract creation request", validationResult);
        }

        var employeeContract = EmployeeContract.Create(
            request.EmployeeId,
            request.ContractType,
            request.EmployeedFrom,
            request.EmployeedTo
        );

        _logger.LogInformation("Creating new employee contract for employee ID: {UserId} started", request.EmployeeId);

        await _employeeRepository.CreateEmployeeContract(employeeContract);

        _logger.LogInformation("Creating new employee contract for employee ID: {UserId} successful", request.EmployeeId);

        return employeeContract.Id;
    }
}
