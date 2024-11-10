using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace HRLeaveManagement.Identity.Adapters;

public class IdentityResultAdapter : IIdentityResult
{
    public IEnumerable<ValidationError> ToValidationErrors(object result)
    {
        if (result is IdentityResult identityResult) 
        {
            return identityResult.Errors
                .Select(error => new ValidationError(error.Code, [error.Description]))
                .ToList();
        }

        throw new InvalidOperationException("Unsupported result type.");
    }
}
