namespace HRLeaveManagement.Application.DTOs.Auth;

public record RegistrationRequest(string FirstName,
                                  string LastName,
                                  string Email,
                                  DateTime DateOfBirth,
                                  string? PeselNumber,
                                  string PhoneNumber,
                                  List<string> Roles);