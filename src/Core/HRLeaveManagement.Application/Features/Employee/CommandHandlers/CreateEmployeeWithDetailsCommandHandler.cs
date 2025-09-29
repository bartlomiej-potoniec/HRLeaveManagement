using DomainEmployee = HRLeaveManagement.Domain.Entities.Employee;
using DomainSection = HRLeaveManagement.Domain.Entities.Section;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Contracts.Persistence;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class CreateEmployeeWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                            ISectionRepository sectionRepository,
                                                            IEmployeeSubservice employeeSubservice,
                                                            IUserService userService,
                                                            IUnitOfWork unitOfWork,
                                                            IAppLogger<CreateEmployeeWithDetailsCommandHandler> logger)
    : IRequestHandler<CreateEmployeeWithDetailsCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IEmployeeSubservice _employeeSubservice = employeeSubservice;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<CreateEmployeeWithDetailsCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateEmployeeWithDetailsCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateEmployeeWithDetailsCommandValidator(_sectionRepository, _userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeWithDetailsCommand));
            throw new BadRequestException("Invalid employee creation request", validationResult);
        }

        var user = await _userService
            .GetUserByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException($"No user with ID: { request.UserId } found");

        var gender = DomainEmployee.MapGender(user.Gender);

        DomainEmployee? leader = default;
        DomainSection? section = default;
        
        if (request.LeaderId is not null)
        {
            leader = await _employeeRepository
                .GetByIdAsync(request.LeaderId.Value, cancellationToken)
                ?? throw new NotFoundException($"No employee with ID: { request.LeaderId } found");            
        }

        if (request.SectionId is not null)
        {
            section = await _sectionRepository
                .GetByIdAsync(request.SectionId.Value, cancellationToken)
                ?? throw new NotFoundException($"No section with ID: { request.SectionId } found");
        }

        var employee = DomainEmployee.Create(
            user.Id,
            user.FirstName,
            user.LastName,
            gender,
            request.Position,
            request.Responsibilities,
            request.ResidentialAddress,
            request.RegisteredAddress,
            request.SecondaryResidentialAddress,
            request.RemoteWorkAddress,
            section,
            leader
        );
        
        await _employeeSubservice.CreateEmployeeContract(employee, request.EmployeeContract, cancellationToken);
        await _employeeSubservice.CreateEmployeeEducations(employee, request.EmployeeEducations, cancellationToken);
        await _employeeSubservice.CreateEmployeeExperiences(employee, request.EmployeeExperiences, cancellationToken);

        _logger.LogInformation("Creating new employee for user ID: {UserId}", request.UserId);
        await _employeeRepository.AddAsync(employee, cancellationToken);
        _logger.LogInformation("Creating new employee successful for user ID: {UserId}", request.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return employee.Id;
    }
}
