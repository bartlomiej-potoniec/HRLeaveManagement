namespace HRLeaveManagement.Application.DTOs.Users;

public record UnlockManyUserAccountsRequest(List<Guid> UserIds);
