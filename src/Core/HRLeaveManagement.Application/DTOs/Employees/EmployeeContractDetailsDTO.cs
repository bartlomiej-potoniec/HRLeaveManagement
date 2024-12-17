using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractDetailsDTO
{
    public required int Id { get; init; }
    public required ContractType ContractType { get; init; }
    public required DateOnly StartedAt { get; init; }
    public DateOnly? ExpiredAt { get; init; }
    public int? TotalDuration { get; init; }

    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
}
