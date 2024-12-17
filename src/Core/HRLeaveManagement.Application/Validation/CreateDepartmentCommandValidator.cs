using HRLeaveManagement.Application.Features.Departments.Commands;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;

namespace HRLeaveManagement.Application.Validation;

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator(IEmployeeRepository employeeRepository)
    {
        
    }
}
