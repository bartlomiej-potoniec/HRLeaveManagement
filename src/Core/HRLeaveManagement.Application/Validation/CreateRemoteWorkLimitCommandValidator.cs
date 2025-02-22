using HRLeaveManagement.Application.Features.RemoteWorkLimit.Commands;
using HRLeaveManagement.Application.Contracts.Persistence;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateRemoteWorkLimitCommandValidator : AbstractValidator<CreateRemoteWorkLimitCommand>
{
    public CreateRemoteWorkLimitCommandValidator(IEmployeeRepository employeeRepository)
    {
        RuleFor(c => c.EmployeeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await employeeRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Employee for given ID does not exist");

        RuleFor(c => c.Year)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("{PropertyName} must be less than or equal to actual year");

        RuleFor(c => c.AvailableDays)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .Must((command, days) =>
            {
                int currentYear = DateTime.UtcNow.Year;
                DateTime startOfYear = new(currentYear, 1, 1);
                DateTime today = DateTime.UtcNow;

                int totalDaysInYear = DateTime.IsLeapYear(currentYear) ? 366 : 365;
                int daysPassedUntilToday = (today - startOfYear).Days + 1;

                int availableDaysInYear = totalDaysInYear - daysPassedUntilToday;

                return !(days > availableDaysInYear);
            })
                .WithMessage("{PropertyName} must be less than or equal to available days in current year");
    }
}
