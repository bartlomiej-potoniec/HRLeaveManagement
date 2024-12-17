using HRLeaveManagement.Application.Features.Departments.Commands;
using FluentValidation;
using HRLeaveManagement.Application.Contracts.Persistence;

namespace HRLeaveManagement.Application.Validation;

public sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator(IEmployeeRepository employeeRepository)
    {
        
    }
}
