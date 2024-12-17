using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceDetailsDTO
{
    public required int Id { get; init; }
    public required ContractType ContractType { get; init; }
    public required DateOnly EmployedFrom { get; init; }
    public required DateOnly EmployedTo { get; init; }
    public required int TotalEmployment { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
