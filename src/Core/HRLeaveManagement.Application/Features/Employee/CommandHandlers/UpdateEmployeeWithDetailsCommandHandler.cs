using DomainEmployee = HRLeaveManagement.Domain.Entities.Employee;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Features.Employee.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.CommandHandlers;

public sealed class UpdateEmployeeWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                            ISectionRepository sectionRepository,
                                                            IUserService userService,
                                                            IAppLogger<UpdateEmployeeWithDetailsCommandHandler> logger) 
    : IRequestHandler<UpdateEmployeeWithDetailsCommand>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ISectionRepository _sectionRepository = sectionRepository;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<UpdateEmployeeWithDetailsCommandHandler> _logger = logger;

    public async Task Handle(UpdateEmployeeWithDetailsCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateEmployeeWithDetailsCommandValidator(
            _employeeRepository,
            _sectionRepository,
            _userService
        );

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateEmployeeWithDetailsCommand));
            throw new BadRequestException("Invalid employee with details update request", validationResult);
        }

        var employeeWithDetails = await _employeeRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No employee with ID: { request.Id } found");

        DomainEmployee.Update(
            employeeWithDetails,
            request.Position,
            request.Responsibilities,
            request.SectionId,
            request.LeaderId
        );

        List<EmployeeEducation> educationsToCreate = [];
        List<EmployeeEducation> educationsToUpdate = [];
        List<EmployeeEducation> educationsToDelete = [];
        List<EmployeeExperience> experiencesToCreate = [];
        List<EmployeeExperience> experiencesToUpdate = [];
        List<EmployeeExperience> experiencesToDelete = [];
        List<EmployeeContract> contractsToCreate = [];
        List<EmployeeContract> contractsToUpdate = [];
        List<EmployeeContract> contractsToDelete = [];

        request.EmployeeEducations.ForEach(ree =>
        {
            var existingEducation = employeeWithDetails.EmployeeEducations.FirstOrDefault(ee => ee.Id == ree.Id);
            
            if (existingEducation is null) 
            {
                educationsToCreate.Add(EmployeeEducation.Create(
                    employeeWithDetails, 
                    ree.EducationType, 
                    ree.EducationDetails, 
                    ree.EnrolledAt, 
                    ree.GraduatedAt
                ));

                return; 
            }

            EmployeeEducation.Update(existingEducation, ree.EducationType, ree.EducationDetails, ree.EnrolledAt, ree.GraduatedAt);
            educationsToUpdate.Add(existingEducation);
        });

        educationsToDelete = employeeWithDetails.EmployeeEducations
            .Except(educationsToUpdate)
            .ToList();

        request.EmployeeExperiences.ForEach(ree => 
        {
            var existingExperience = employeeWithDetails.EmployeeExperiences.FirstOrDefault(ee => ee.Id == ree.Id);

            if (existingExperience is null)
            {
                experiencesToCreate.Add(EmployeeExperience.Create(
                    employeeWithDetails,
                    ree.ContractType,
                    ree.PreviousCompanyName,
                    ree.Position,
                    ree.EmployedFrom,
                    ree.EmployedTo
                ));

                return;
            }

            EmployeeExperience.Update(
                existingExperience,
                ree.ContractType,
                ree.PreviousCompanyName,
                ree.Position,
                ree.EmployedFrom,
                ree.EmployedTo
            );

            experiencesToUpdate.Add(existingExperience);
        });

        experiencesToDelete = employeeWithDetails.EmployeeExperiences
            .Except(experiencesToUpdate)
            .ToList();

        request.EmployeeContracts.ForEach(rec =>
        {
            var existingContracts = employeeWithDetails.EmploymentContracts.FirstOrDefault(ec => ec.Id == rec.Id);

            if (existingContracts is null)
            {
                contractsToCreate.Add(EmployeeContract.Create(
                    employeeWithDetails,
                    rec.ContractType,
                    rec.EmployeedFrom,
                    rec.EmployeedTo
                ));

                return;
            }

            EmployeeContract.Update(existingContracts, rec.ContractType, rec.EmployeedFrom, rec.EmployeedTo);
            contractsToUpdate.Add(existingContracts);
        });

        contractsToDelete = employeeWithDetails.EmploymentContracts
            .Except(contractsToUpdate)
            .ToList();

        await _employeeRepository.UpdateWithDetailsAsync(
            employeeWithDetails,
            contractsToCreate,
            educationsToCreate,
            experiencesToCreate,
            contractsToUpdate,
            educationsToUpdate,
            experiencesToUpdate,
            contractsToDelete,
            educationsToDelete,
            experiencesToDelete
        );
    }
}
