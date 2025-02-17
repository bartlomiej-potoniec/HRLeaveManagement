namespace HRLeaveManagement.Application.DTOs.Users;

public record UpdateUserRequest(Guid Id,
                                string FirstName,
                                string LastName,
                                string Email,
                                DateTime DateOfBirth,
                                string? PeselNumber,
                                string PhoneNumber,
                                List<string> Roles);
