using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Features.Employee.Commands;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateEmployeeWithDetailsCommandValidator : AbstractValidator<CreateEmployeeWithDetailsCommand>
{
    public CreateEmployeeWithDetailsCommandValidator(ISectionRepository sectionRepository,
                                                     IUserService userService)
    {
        RuleFor(c => c.UserId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await userService.GetUserByIdAsync(id, token) is not null)
                .WithMessage("User for given ID does not exist")
            .MustAsync(async (id, token) => (await userService.GetUserByIdAsync(id, token)).EmployeeId is null)
                .WithMessage("Employee account for user with given ID already exist");

        RuleFor(c => c.Position)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.Responsibilities)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty");

        RuleFor(c => c.SectionId)
            .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0")
            .MustAsync(async (id, token) => id is null || await sectionRepository.GetByIdAsync(id.Value, token) is not null)
                .WithMessage("Section for given ID does not exist");

        RuleFor(c => c.LeaderId)
            .MustAsync(async (id, token) => id is null || await userService.IsUserInManagerRoleByEmployeeIdAsync(id.Value, token))
                .WithMessage("Leader for given ID does not exist");


        RuleFor(c => c.EmployeeContract.ContractType)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .IsInEnum()
                .WithMessage(c =>
                {
                    var validValues = string.Join(", ", Enum.GetValues<ContractType>());
                    return $"Value of ContractType must be in [{ validValues }]";
                });

        RuleFor(c => c.EmployeeContract.EmployeedFrom)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .LessThan(c => c.EmployeeContract.EmployeedTo)
                .When(c => c.EmployeeContract.EmployeedTo.HasValue)
                    .WithMessage("EmployeedFrom must be less than EmployeedFrom, if specified.");

        RuleFor(c => c.EmployeeContract.EmployeedTo)
            .GreaterThan(c => c.EmployeeContract.EmployeedFrom)
                .When(c => c.EmployeeContract.EmployeedTo.HasValue)
                    .WithMessage("EmployeedTo must be greater than EmployeedFrom, if specified.");


        RuleForEach(c => c.EmployeeEducations)
            .ChildRules(education =>
            {
                education.RuleFor(e => e.EducationType)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .IsInEnum()
                        .WithMessage(c =>
                        {
                            var validValues = string.Join(", ", Enum.GetValues<EducationType>());
                            return $"Value of EducationType must be in [{ validValues }]";
                        });

                education.RuleFor(c => c.EducationDetails)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .NotEmpty()
                        .WithMessage("{PropertyName} cannot be empty");

                education.RuleFor(c => c.EnrolledAt)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .LessThan(c => c.GraduatedAt)
                        .When(c => c.GraduatedAt.HasValue)
                            .WithMessage("EnrolledAt must be less than GraduatedAt, if specified.");

                education.RuleFor(c => c.GraduatedAt)
                    .GreaterThan(c => c.EnrolledAt)
                        .When(c => c.GraduatedAt.HasValue)
                            .WithMessage("GraduatedAt must be greater than EnrolledAt, if specified.");
            });


        RuleForEach(c => c.EmployeeExperiences)
            .ChildRules(experience =>
            {
                experience.RuleFor(e => e.ContractType)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .IsInEnum()
                        .WithMessage(c =>
                        {
                            var validValues = string.Join(", ", Enum.GetValues<ContractType>());
                            return $"Value of ContractType must be in [{ validValues }]";
                        });

                experience.RuleFor(c => c.EmployedFrom)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .LessThan(c => c.EmployedTo)
                        .WithMessage("EmployedFrom must be less than EmployedTo, if specified.");

                experience.RuleFor(c => c.EmployedTo)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .GreaterThan(c => c.EmployedFrom)
                        .WithMessage("EmployedTo must be greater than EmployedFrom, if specified.");
            });
    }
}
