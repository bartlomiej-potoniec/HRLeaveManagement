using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using FluentValidation;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EditEmployeeDetailsViewModelValidator 
    : AbstractValidator<EmployeeDetailsViewModel>, IViewModelValidator<EmployeeDetailsViewModel>
{
    public EditEmployeeDetailsViewModelValidator()
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
    }
}
