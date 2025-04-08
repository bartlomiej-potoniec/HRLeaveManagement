using HRLeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRLeaveManagement.Api.Filters;

public sealed class ValidateGuidFilterAttribute(params string[] parameters) : ActionFilterAttribute
{
    private readonly List<string> _parameters = [.. parameters];

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _parameters.ForEach(param =>
        {
            var isValueInParams = context.ActionArguments.TryGetValue(param, out var value);

            if (!isValueInParams)
            {
                return;
            }

            var isValidGuid = Guid.TryParse(value?.ToString(), out Guid validGuid);

            if (!isValidGuid)
            {
                throw new BadRequestException($"Given value { value } is not a valid Guid");
            }
        });

        base.OnActionExecuting(context);
    }
}
