namespace HRLeaveManagement.Application.DTOs.Identity;

public sealed record UserDTO(string Id,
                             string Email,
                             string FirstName,
                             string LastName,
                             string? PeselNumber,
                             DateOnly DateOfBirth,
                             Guid? EmployeeId);
