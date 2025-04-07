using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Extensions;
using HRLeaveManagement.BlazorUI.ViewModels.Employees;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class EmployeeEducationViewModelValidator 
    : AbstractValidator<EmployeeEducationViewModel>, IViewModelValidator<EmployeeEducationViewModel>
{
    public EmployeeEducationViewModelValidator()
    {
        RuleFor(x => x.EducationType)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.EducationType);

        RuleFor(x => x.EducationDetails)
            .NotNull()
            .NotEmpty()
            .MaximumLength(200)
                .WithDisplayName(x => x.EducationDetails);

        RuleFor(x => x.EnrolledAt)
            .NotNull()
            .NotEmpty()
                .WithDisplayName(x => x.EnrolledAt);

        RuleFor(x => x.EnrolledAt)
            .LessThanOrEqualTo(x => x.GraduatedAt)
                .WithDisplayName(x => x.EnrolledAt)
                    .When(x => x.GraduatedAt.HasValue);

        RuleFor(x => x.GraduatedAt)
            .NotNull()
                .WithMessage("Pole '{PropertyName}' może być puste tylko jeśli pracownik wciąż się uczy")
            .NotEmpty()
                .WithMessage("Pole '{PropertyName}' może być puste tylko jeśli pracownik wciąż się uczy")
                .WithDisplayName(x => x.GraduatedAt)
                    .When(x => !(x.IsEmployeeStillStudying));
                    
        RuleFor(x => x.GraduatedAt)
            .LessThanOrEqualTo(DateTime.Now)
                .WithDisplayName(x => x.GraduatedAt)
                    .When(x => x.GraduatedAt.HasValue);
    }
}
