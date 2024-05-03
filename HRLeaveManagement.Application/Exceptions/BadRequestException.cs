using FluentValidation.Results;

namespace HRLeaveManagement.Application.Exceptions;

internal class BadRequestException : Exception
{
    public List<string> ValidationErrors { get; set; } = [];

    internal BadRequestException(string message) : base(message) {}

    internal BadRequestException(string message, ValidationResult result) : base(message) 
        => result.Errors
            .ForEach(error => ValidationErrors.Add(error.ErrorMessage));
}
