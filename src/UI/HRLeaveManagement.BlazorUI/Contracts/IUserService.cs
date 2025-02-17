using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Contracts;

public interface IUserService
{
    Task<List<UserDetailsViewModel>> GetAllAsync();
    Task<PagedViewModel<UserDetailsViewModel>> GetAllAsync(int? pageNumber = null,
                                                           int? pageSize = null,
                                                           string? sorts = null,
                                                           string? filters = null);
    Task<UserDetailsViewModel> GetWithDetailsByIdAsync(Guid Id);

    Task<Response> UpdateAsync(Guid id,
                               string firstName,
                               string lastName,
                               string email,
                               DateTime dateOfBirth,
                               string? peselNumber,
                               string phoneNumber,
                               List<string> roles);
    Task<Response> LockoutAsync(Guid id, DateTime lockoutEnd);
    Task<Response> UnlockAsync(Guid id);
}
