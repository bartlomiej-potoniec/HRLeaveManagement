using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using FluentValidation;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Commands;

namespace HRLeaveManagement.Application.Features.Employee.UpdateEmployee.Validation;

public sealed class UpdateEmployeeWithDetailsCommandValidator : AbstractValidator<UpdateEmployeeWithDetailsCommand>
{
    public UpdateEmployeeWithDetailsCommandValidator(IEmployeeRepository employeeRepository,
                                                     ISectionRepository sectionRepository,
                                                     IUserService userService)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required");

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
                .WithMessage("{PropertyName} must be greater than 0");

        RuleFor(c => c.EmployeeContracts)
            .Must(contracts =>
                !contracts.Any(edu1 =>
                    contracts.Any(edu2 =>
                        edu1 != edu2 &&
                        edu1.StartedAt < (edu2.ExpiredAt ?? DateTime.MaxValue) &&
                        (edu1.ExpiredAt ?? DateTime.MaxValue) > edu2.ExpiredAt
                    )
                )
            )
                .WithMessage("Contracts periods cannot overlap");

        RuleForEach(c => c.EmployeeContracts)
            .ChildRules(contracts =>
            {
                contracts.RuleFor(c => c.ContractType)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .IsInEnum()
                        .WithMessage(c =>
                        {
                            var validValues = string.Join(", ", Enum.GetValues<ContractType>());
                            return $"Value of ContractType must be in [{validValues}]";
                        });

                contracts.RuleFor(c => c.StartedAt)
                    .NotNull()
                        .WithMessage("{PropertyName} is required")
                    .LessThan(c => c.ExpiredAt)
                        .When(c => c.ExpiredAt.HasValue)
                            .WithMessage("EmployeedFrom must be less than EmployeedFrom, if specified.");

                contracts.RuleFor(c => c.ExpiredAt)
                    .GreaterThan(c => c.StartedAt)
                        .When(c => c.ExpiredAt.HasValue)
                            .WithMessage("EmployeedTo must be greater than EmployeedFrom, if specified.");
            });


        RuleFor(c => c.EmployeeEducations)
            .Must(educations => 
                educations
                    .Where(e => e.Id.HasValue)
                    .GroupBy(e => e.Id)
                    .All(group => group.Count() == 1)
            )
                .WithMessage("Each {PropertyName} must have a unique Id");

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
                            return $"Value of EducationType must be in [{validValues}]";
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


        RuleFor(c => c.EmployeeExperiences)
            .Must(experiences => experiences
                .Where(e => e.Id.HasValue)
                .GroupBy(e => e.Id)
                .All(group => group.Count() == 1)
            )
                .WithMessage("Each {PropertyName} must have a unique Id");

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
                            return $"Value of ContractType must be in [{validValues}]";
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
