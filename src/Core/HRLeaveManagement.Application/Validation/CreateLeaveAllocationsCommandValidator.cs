using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Contracts.Persistence;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateLeaveAllocationsCommandValidator : AbstractValidator<CreateLeaveAllocationsCommand>
{
    public CreateLeaveAllocationsCommandValidator(ILeaveAllocationRepository leaveAllocationRepository,
                                                  ILeaveTypeRepository leaveTypeRepository,
                                                  IEmployeeRepository employeeRepository)
    {
        RuleFor(c => c.EmployeeId)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .MustAsync(async (id, token) => await employeeRepository.GetByIdAsync(id) is not null)
                .WithMessage("Employee for given ID does not exist");

        RuleFor(c => c.Year)
            .NotNull()
                .WithMessage("{PropertyName} is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("{PropertyName} must be less than or equal to actual year");

        RuleFor(c => c.LeaveAllocations)
            .Custom((requests, context) =>
            {
                var duplicates = requests
                    .GroupBy(r => r.LeaveTypeId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicates.Any())
                    context.AddFailure(
                        "LeaveAllocations",
                        $"Duplicate LeaveTypeId(s) found for entities with ID: { string.Join(", ", duplicates) }"
                    );
            });

        RuleForEach(c => c.LeaveAllocations)
            .ChildRules(allocation =>
            {
                allocation
                    .RuleFor(a => a.LeaveTypeId)
                        .NotNull()
                            .WithMessage("{PropertyName} is required")
                        .MustAsync(async (id, token) => await leaveTypeRepository.GetByIdAsync(id) is not null)
                            .WithMessage("Leave Type for given ID does not exist");

                allocation
                    .RuleFor(a => a.AvailableDays)
                        .GreaterThan(0)
                            .WithMessage("{PropertyName} must be greater than 0");
            })
            .CustomAsync(async (allocation, context, token) =>
            {
                var command = context.InstanceToValidate;

                bool isAllocationForEmployeeExist = await leaveAllocationRepository
                    .IsAllocationForEmployeeExistAsync(
                        command.EmployeeId,
                        allocation.LeaveTypeId,
                        command.Year
                    );

                if (isAllocationForEmployeeExist)
                    context.AddFailure(
                        nameof(command.LeaveAllocations),
                        "Leave Allocation for given Employee, Leave and Year already exists"
                    );
            });
    }
}
