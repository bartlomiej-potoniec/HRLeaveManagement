using DomainEmployee =  HRLeaveManagement.Domain.Entities.Employee;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using System.Transactions;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class CreateEmployeeWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                            ISectionRepository sectionRepository,
                                                            IUserService userService,
                                                            IEmailService emailService,
                                                            IAppLogger<CreateEmployeeWithDetailsCommandHandler> logger)
    : IRequestHandler<CreateEmployeeWithDetailsCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    private readonly IAppLogger<CreateEmployeeWithDetailsCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateEmployeeWithDetailsCommand request,
                                   CancellationToken cancellationToken)
    {
        var validator = new CreateEmployeeWithDetailsCommandValidator(_sectionRepository, _userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeWithDetailsCommand));
            throw new BadRequestException("Invalid employee creation request", validationResult);
        }

        var user = await _userService.GetUserById(request.UserId);

        var employee = DomainEmployee.Create(
            request.Position,
            request.Responsibilities,
            request.SectionId,
            request.LeaderId
        );

        var employeeContract = EmployeeContract.Create(
            employee,
            request.EmployeeContract.ContractType,
            request.EmployeeContract.EmployeedFrom,
            request.EmployeeContract.EmployeedTo
        );

        var employeeEducations = request.EmployeeEducations
            .Select(ee => EmployeeEducation.Create(
                employee,
                ee.EducationType,
                ee.EducationDetails,
                ee.EnrolledAt,
                ee.GraduatedAt
            ))
            .ToList();

        var employeeExperiences = request.EmployeeExperiences
            .Select(ee => EmployeeExperience.Create(
                employee,
                ee.ContractType,
                ee.PreviousCompanyName,
                ee.Position,
                ee.EmployedFrom,
                ee.EmployedTo
            ))
            .ToList();

        _logger.LogInformation("Starting transaction for creating new employee for user ID: {UserId}", request.UserId);
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            _logger.LogInformation("Creating new employee for user ID: {UserId}", request.UserId);

            await _employeeRepository.CreateWithDetailsAsync(
                employee,
                employeeContract,
                employeeEducations,
                employeeExperiences
            );

            _logger.LogInformation("Creating new employee successful for user ID: {UserId}", request.UserId);
            _logger.LogInformation("Updating new employee with ID: {EmployeeId} with user ID: {UserId}", employee.Id, request.UserId);

            await _userService.UpdateUserEmployeeId(request.UserId, employee.Id);

            _logger.LogInformation("Updating new employee with ID: {EmployeeId} with user ID: {UserId} successful", employee.Id, request.UserId);

            await _emailService.SendEmployeeCreationEmail(user.Email, user.FirstName);

            transactionScope.Complete();
            _logger.LogInformation("Transaction successful for creating new employee for user ID: {UserId}", request.UserId);

            return employee.Id;
        }

        catch (Exception ex)
        {
            _logger.LogError("Transaction failed for creating new employee for user ID: {UserId}", request.UserId);
            throw;
        }
    }
}
