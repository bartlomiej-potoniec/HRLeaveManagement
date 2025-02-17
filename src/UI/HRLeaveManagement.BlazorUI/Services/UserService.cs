using AutoMapper;
using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class UserService(IClient client,
                                ILocalStorageService localStorage,
                                IMapper mapper)
    : HttpServiceBase(client, localStorage), IUserService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserDetailsViewModel>> GetAllAsync()
    {
        var pagedUsers = await _client.UsersGETAsync(null, null, null, null);
        var viewModel = _mapper.Map<List<UserDetailsViewModel>>(pagedUsers.Items);

        return viewModel;
    }

    public async Task<PagedViewModel<UserDetailsViewModel>> GetAllAsync(int? pageNumber = null,
                                                                        int? pageSize = null,
                                                                        string? sorts = null,
                                                                        string? filters = null)
    {
        var pagedUsers = await _client.UsersGETAsync(pageNumber, pageSize, sorts, filters);
        var viewModel = _mapper.Map<PagedViewModel<UserDetailsViewModel>>(pagedUsers);

        return viewModel;
    }

    public async Task<UserDetailsViewModel> GetWithDetailsByIdAsync(Guid id)
    {
        var user = await _client.UsersGET2Async(id);
        var viewModel = _mapper.Map<UserDetailsViewModel>(user);

        return viewModel;
    }

    public async Task<Response> UpdateAsync(Guid id,
                                            string firstName,
                                            string lastName,
                                            string email,
                                            DateTime dateOfBirth,
                                            string? peselNumber,
                                            string phoneNumber,
                                            List<string> roles)
    {
        Response response;

        try
        {
            var request = new UpdateUserRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                PeselNumber = peselNumber,
                PhoneNumber = phoneNumber,
                Roles = roles
            };


            await _client.UsersPUTAsync(id, request);
            response = base.GenerateSuccessResponse("User updated successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;        
    }

    public async Task<Response> LockoutAsync(Guid id, DateTime lockoutEnd)
    {
        Response response;

        try
        {
            var request = new LockoutUserAccountRequest 
            { 
                UserId = id,
                LockoutEnd = lockoutEnd
            };

            await _client.LockoutAsync(id, request);
            response = base.GenerateSuccessResponse("User locked out successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> UnlockAsync(Guid id)
    {
        Response response;

        try
        {
            var request = new UnlockUserAccountRequest { UserId = id };

            await _client.UnlockAsync(id, request);
            response = base.GenerateSuccessResponse("User unlocked successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }
}
