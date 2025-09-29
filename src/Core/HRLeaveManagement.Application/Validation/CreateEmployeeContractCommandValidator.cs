using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.Employee.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateEmployeeContractCommandValidator : AbstractValidator<CreateEmployeeContractCommand>
{
    public CreateEmployeeContractCommandValidator(IEmployeeRepository employeeRepository)
    {
        RuleFor(c => c.EmployeeId)
            .NotNull()
                .WithMessage("{PropertyName} is required");

        RuleFor(c => c.ContractType)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .IsInEnum()
                .WithMessage(c =>
                {
                    var validValues = string.Join(", ", Enum.GetValues<ContractType>());
                    return $"Value of ContractType must be in [{validValues}]";
                });

        RuleFor(c => c.StartedAt)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .LessThan(c => c.ExpiredAt)
                .When(c => c.ExpiredAt.HasValue)
                    .WithMessage("EmployeedFrom must be less than EmployeedFrom, if specified.");

        RuleFor(c => c.ExpiredAt)
            .GreaterThan(c => c.StartedAt)
                .When(c => c.ExpiredAt.HasValue)
                    .WithMessage("EmployeedTo must be greater than EmployeedFrom, if specified.");
    }
}
