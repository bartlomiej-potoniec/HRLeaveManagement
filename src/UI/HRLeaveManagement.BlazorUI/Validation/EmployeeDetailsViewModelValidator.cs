using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeDetailsViewModelValidator 
    : AbstractValidator<CreateEmployeeDetailsViewModel>, IViewModelValidator<CreateEmployeeDetailsViewModel>
{
    public EmployeeDetailsViewModelValidator()
    {
        RuleFor(x => x.Position)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
                .WithDisplayName(x => x.Position);

        RuleFor(x => x.Responsibilities)
            .NotNull()
            .NotEmpty()
            .MaximumLength(1000)
                .WithDisplayName(x => x.Responsibilities);

        RuleFor(x => x.SectionId)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.SectionId);

        RuleFor(x => x.LeaderId)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.LeaderId);

        RuleFor(x => x.Contract.ContractType)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.Contract.ContractType);

        RuleFor(x => x.Contract.StartedAt)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.Contract.StartedAt);

        RuleFor(x => x.Contract.StartedAt)
            .LessThan(x => x.Contract.ExpiredAt)
                .WithDisplayName(x => x.Contract.StartedAt)
                    .When(x => x.Contract.ExpiredAt.HasValue);

        RuleFor(x => x.Contract.ExpiredAt)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.Contract.ExpiredAt)
                    .When(x => !(x.Contract.IsContractForIndefinitePeriod));

        RuleFor(x => x.Contract.ExpiredAt)
            .GreaterThan(x => x.Contract.StartedAt)
                .WithDisplayName(x => x.Contract.ExpiredAt)
                    .When(x => x.Contract.ExpiredAt.HasValue && x.Contract.StartedAt.HasValue);
    }
} 
