using HRLeaveManagement.Application.Features.Employee.Commands;
using FluentValidation;
using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateEmployeeContractCommandValidator : AbstractValidator<CreateEmployeeContractCommand>
{
    public CreateEmployeeContractCommandValidator(IEmployeeRepository employeeRepository)
    {
        RuleFor(c => c.EmployeeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await employeeRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Employee for given ID does not exist");

        RuleFor(c => c.ContractType)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .IsInEnum()
                .WithMessage(c =>
                {
                    var validValues = string.Join(", ", Enum.GetValues<ContractType>());
                    return $"Value of ContractType must be in [{validValues}]";
                });

        RuleFor(c => c.EmployeedFrom)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .LessThan(c => c.EmployeedTo)
                .When(c => c.EmployeedTo.HasValue)
                    .WithMessage("EmployeedFrom must be less than EmployeedFrom, if specified.");

        RuleFor(c => c.EmployeedTo)
            .GreaterThan(c => c.EmployeedFrom)
                .When(c => c.EmployeedTo.HasValue)
                    .WithMessage("EmployeedTo must be greater than EmployeedFrom, if specified.");
    }
}
