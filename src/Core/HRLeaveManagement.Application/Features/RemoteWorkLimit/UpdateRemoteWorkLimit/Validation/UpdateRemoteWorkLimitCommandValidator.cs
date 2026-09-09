using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Features.RemoteWorkLimit.UpdateRemoteWorkLimit.Commands;

namespace HRLeaveManagement.Application.Features.RemoteWorkLimit.UpdateRemoteWorkLimit.Validation;

public sealed class UpdateRemoteWorkLimitCommandValidator : AbstractValidator<UpdateRemoteWorkLimitCommand>
{
    public UpdateRemoteWorkLimitCommandValidator(IEmployeeRepository employeeRepository,
                                                 IRemoteWorkLimitRepository remoteWorkLimitRepository)
    {
        RuleFor(c => c.Id)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await remoteWorkLimitRepository.GetByIdAsync(id, token) is not null)
                .WithMessage("Remote work limit for given ID does not exist");

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
