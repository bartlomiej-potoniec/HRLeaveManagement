using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.Employee.Commands;
using FluentValidation;

namespace HRLeaveManagement.Application.Validation;

public class CreateEmployeeContractCommandValidator : AbstractValidator<CreateEmployeeContractCommand>
{
    public CreateEmployeeContractCommandValidator(IEmployeeRepository employeeRepository)
    {
        
    }
}
