namespace HRLeaveManagement.Application.DTOs.Users;

public record LockoutManyUserAccountsRequest(List<Guid> UserIds, DateTime LockoutEnd);
