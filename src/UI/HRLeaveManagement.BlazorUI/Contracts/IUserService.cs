using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IUserService
{
    Task<Response<List<UserDetailsViewModel>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Response<PagedViewModel<UserDetailsViewModel>>> GetAllAsync(int? pageNumber = null,
                                                                     int? pageSize = null,
                                                                     string? sorts = null,
                                                                     string? filters = null,
                                                                     CancellationToken cancellationToken = default);
    Task<Response<UserDetailsViewModel>> GetWithDetailsByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Response> UpdateAsync(Guid id,
                               string firstName,
                               string lastName,
                               string email,
                               DateTime dateOfBirth,
                               string? peselNumber,
                               string phoneNumber,
                               List<string> roles,
                               CancellationToken cancellationToken = default);
    Task<Response> DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Response> DeleteManyAsync(List<Guid> usersId, CancellationToken cancellationToken = default);
    Task<Response> LockoutAsync(Guid id, DateTime lockoutEnd, CancellationToken cancellationToken = default);
    Task<Response> LockoutManyAsync(List<Guid> ids, DateTime lockoutEnd, CancellationToken cancellationToken = default);
    Task<Response> UnlockAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Response> UnlockManyAsync(List<Guid> usersId, CancellationToken cancellationToken = default);
}
