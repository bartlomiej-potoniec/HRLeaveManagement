using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class CreateEmployeeContractCommandHandler(IEmployeeRepository employeeRepository,
                                                         IEmployeeSubservice employeeSubservice,
                                                         IUnitOfWork unitOfWork,
                                                         IAppLogger<CreateEmployeeContractCommandHandler> logger)
    : IRequestHandler<CreateEmployeeContractCommand, int>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IEmployeeSubservice _employeeSubservice = employeeSubservice;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<CreateEmployeeContractCommandHandler> _logger = logger;

    public async Task<int> Handle(CreateEmployeeContractCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateEmployeeContractCommandValidator(_employeeRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeContractCommand));
            throw new BadRequestException("Invalid employee contract creation request", validationResult);
        }

        var employee = await _employeeRepository
            .GetWithContractsByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { request.EmployeeId } found");

        var employeeContractRequest = new EmployeeContractRequest(
            request.ContractType,
            request.StartedAt,
            request.ExpiredAt,
            request.ContractDetails,
            request.EmployeeDocuments
        );

        var employeeContract = await _employeeSubservice
            .CreateEmployeeContract(employee, employeeContractRequest, cancellationToken);

        _logger.LogInformation("Creating new employee contract for employee ID: {UserId} started", request.EmployeeId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new employee contract for employee ID: {UserId} successful", request.EmployeeId);

        // sprawdzić czy EF dobrze śledzi zmiany i automatycznie załadował auto-inkrementowane ID
        return employeeContract.Id;
    }
}
