using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using FluentValidation;
using MediatR;
using DomainEmployee = HRLeaveManagement.Domain.Employee.Employee;
using HRLeaveManagement.Domain.Outbox;

namespace HRLeaveManagement.Application.Features.Employee.CreateEmployee.Commands;

public sealed class CreateEmployeeWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                            ISectionRepository sectionRepository,
                                                            IEmployeeSubservice employeeSubservice,
                                                            IUserService userService,
                                                            IUnitOfWork unitOfWork,
                                                            IRepository repository,
                                                            IAppLogger<CreateEmployeeWithDetailsCommandHandler> logger)
    : IRequestHandler<CreateEmployeeWithDetailsCommand, Guid>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IEmployeeSubservice _employeeSubservice = employeeSubservice;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRepository _repository = repository;
    private readonly IAppLogger<CreateEmployeeWithDetailsCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateEmployeeWithDetailsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new employee for user ID: {UserId}", request.UserId);

        var validator = new CreateEmployeeWithDetailsCommandValidator(_sectionRepository, _userService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _userService
            .GetUserByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException($"No user with ID: { request.UserId } found");

        var leader = request.LeaderId.HasValue
            ? await _employeeRepository.GetByIdAsync(request.LeaderId.Value, cancellationToken) 
            : null;

        var section = request.SectionId.HasValue 
            ? await _sectionRepository.GetByIdAsync(request.SectionId.Value, cancellationToken) 
            : null;

        var employee = DomainEmployee.Create(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Gender,
            request.Position,
            request.Responsibilities,
            request.ResidentialAddress,
            request.RegisteredAddress,
            request.SecondaryResidentialAddress,
            request.RemoteWorkAddress,
            section,
            leader);
        
        await _employeeSubservice.CreateEmployeeContract(employee, request.EmployeeContract, cancellationToken);
        await _employeeSubservice.CreateEmployeeEducations(employee, request.EmployeeEducations, cancellationToken);
        await _employeeSubservice.CreateEmployeeExperiences(employee, request.EmployeeExperiences, cancellationToken);

        await _repository.AddAsync(employee, cancellationToken);

        OutboxMessage.Create(
            new UpdateIdentityEmployeeIdAfterCreationEvent(user.Id, employee.Id),
            new AllocateLeavesAfterEmployeeCreationEvent(employee.Id),
            new SendEmployeeCreationEmailEvent(employee.Id, employee.FirstName, employee.LastName, user.Email));

        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Creating new employee successful for user ID: {UserId}", request.UserId);
        return employee.Id;
    }
}

public sealed record UpdateIdentityEmployeeIdAfterCreationEvent(Guid UserId, Guid EmployeeId)
    : INotification;

public sealed record SendEmployeeCreationEmailEvent(Guid EmployeeId, string FirstName, string LastName, string Email)
    : INotification;

public sealed record AllocateLeavesAfterEmployeeCreationEvent(Guid EmployeeId)
    : INotification;

public interface IRepository
{
    Task<IEnumerable<TEntity>> GetAsync<TEntity>(CancellationToken cancellationToken);
    Task<TEntity?> FirstOrDefaultAsync<TEntity, TKey>(TKey type, CancellationToken cancellationToken);
    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
