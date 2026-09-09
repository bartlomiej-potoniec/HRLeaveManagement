using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceDetailsDTO
{
    public required int Id { get; init; }
    public required ContractType ContractType { get; init; }
    public required string PreviousCompanyName { get; init; }
    public required string Position { get; init; }
    public required DateTime EmployedFrom { get; init; }
    public required DateTime EmployedTo { get; init; }
    public required int TotalEmployment { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
