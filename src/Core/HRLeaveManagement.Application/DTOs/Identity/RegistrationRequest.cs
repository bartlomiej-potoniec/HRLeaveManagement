namespace HRLeaveManagement.Application.DTOs.Identity;

public sealed record RegistrationRequest(string FirstName,
                                         string LastName,
                                         string Email,
                                         DateTime DateOfBirth,
                                         string? PeselNumber,
                                         List<string> Roles);