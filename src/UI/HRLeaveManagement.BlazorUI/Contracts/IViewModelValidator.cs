using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IViewModelValidator<T> : IValidator<T> where T : new()
{
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue
        => async (model, propertyName)
            =>
                {
                    var result = await ValidateAsync(ValidationContext<T>
                        .CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));

                    if (result.IsValid)
                    {
                        return [];
                    }

                    return result.Errors.Select(e => e.ErrorMessage);
                };
}
