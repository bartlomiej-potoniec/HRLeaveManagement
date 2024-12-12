namespace HRLeaveManagement.Application.DTOs.Identity;

public sealed record PasswordRequest(string CurrentPassword, string NewPassword);
