using HRLeaveManagement.Application.Exceptions;

namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IIdentityResult
{
    IEnumerable<ValidationError> ToValidationErrors(object result);
}
