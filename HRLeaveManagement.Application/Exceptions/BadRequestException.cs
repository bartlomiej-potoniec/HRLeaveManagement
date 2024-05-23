using FluentValidation.Results;

namespace HRLeaveManagement.Application.Exceptions;

public class BadRequestException : Exception
{
    public IDictionary<string, string[]> ValidationErrors { get; set; } 
        = (Dictionary<string, string[]>)[];

    internal BadRequestException(string message) : base(message) {}

    internal BadRequestException(string message, ValidationResult validationResult) : base(message)
        => ValidationErrors = validationResult.ToDictionary();
}
