namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeDetailsDTO
{
    public required Guid UserId { get; init; }
    public required Guid EmployeeId { get; init; }

    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? PeselNumber { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateTime DateOfBirth { get; init; }

    public required string Position { get; init; }
    public required int SectionId { get; init; }
    public required string Section { get; init; }
    public required int DepartmentId { get; init; }
    public required string Department { get; init; }
    public required string Responsibilities { get; init; }

    public bool IsCurrentlyEmployed { get; init; }
    public List<EmployeeContractDetailsDTO> Contracts { get; init; } = [];
    public List<EmployeeEducationDetailsDTO> Educations { get; init; } = [];
    public List<EmployeeExperienceDetailsDTO> Experiences { get; init; } = [];

    public Guid? LeaderId { get; init; }
    public string? LeaderName { get; init; }
}
