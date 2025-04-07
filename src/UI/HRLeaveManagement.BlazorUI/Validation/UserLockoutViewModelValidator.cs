using HRLeaveManagement.BlazorUI.ViewModels.Users;
using HRLeaveManagement.BlazorUI.Contracts;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Validation;

public class UserLockoutViewModelValidator 
    : AbstractValidator<UserLockoutViewModel>, IViewModelValidator<UserLockoutViewModel>
{
    public UserLockoutViewModelValidator()
    {
        RuleFor(x => x.LockoutDateEnd)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.Date);

        RuleFor(x => x.LockoutTimeEnd)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.TimeOfDay);
    }
}
