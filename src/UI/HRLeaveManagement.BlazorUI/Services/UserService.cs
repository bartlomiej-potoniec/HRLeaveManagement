using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class UserService(IClient client,
                                ILocalStorageService localStorage,
                                IMapper mapper)
    : HttpServiceBase(client, localStorage), IUserService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response<List<UserDetailsViewModel>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        Response<List<UserDetailsViewModel>> response;

        try
        {
            var pagedUsers = await _client.UsersGETAsync(null, null, null, null, cancellationToken);
            var viewModel = _mapper.Map<List<UserDetailsViewModel>>(pagedUsers.Items);

            response = base.GenerateSuccessResponse("Pomyślnie pobrano dane użytkowników", viewModel);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<List<UserDetailsViewModel>>(ex);
        }

        return response;
    }

    public async Task<Response<PagedViewModel<UserDetailsViewModel>>> GetAllAsync(int? pageNumber = null,
                                                                                  int? pageSize = null,
                                                                                  string? sorts = null,
                                                                                  string? filters = null,
                                                                                  CancellationToken cancellationToken = default)
    {
        Response<PagedViewModel<UserDetailsViewModel>> response;

        try
        {
            var pagedUsers = await _client.UsersGETAsync(pageNumber, pageSize, sorts, filters, cancellationToken);
            var viewModel = _mapper.Map<PagedViewModel<UserDetailsViewModel>>(pagedUsers);

            response = base.GenerateSuccessResponse("Pomyślnie pobrano dane użytkowników", viewModel);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<PagedViewModel<UserDetailsViewModel>>(ex);
        }

        return response;
    }

    public async Task<Response<UserDetailsViewModel>> GetWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Response<UserDetailsViewModel> response;

        try
        {
            var user = await _client.UsersGET2Async(id, cancellationToken);
            var viewModel = _mapper.Map<UserDetailsViewModel>(user);

            response = base.GenerateSuccessResponse("Pomyślnie pobrano dane użytkownika", viewModel);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<UserDetailsViewModel>(ex);
            Console.WriteLine(ex.Message);
        }

        return response;
    }

    public async Task<Response> UpdateAsync(Guid id,
                                            string firstName,
                                            string lastName,
                                            string email,
                                            DateTime dateOfBirth,
                                            string? peselNumber,
                                            string phoneNumber,
                                            List<string> roles,
                                            CancellationToken cancellationToken = default)
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

            await _client.UsersPUTAsync(id, request, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie zaktualizowane dane użytkownika");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;        
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            await _client.UsersDELETE2Async(id, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie usunięto użytkownika");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            await _client.UsersDELETEAsync(ids, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie usunięto wybranych użytkowników");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> LockoutAsync(Guid id,
                                             DateTime lockoutEnd,
                                             CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            var request = new LockoutUserAccountRequest 
            { 
                UserId = id,
                LockoutEnd = lockoutEnd
            };

            await _client.LockoutAsync(id, request, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie zablokowano użytkownika");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> LockoutManyAsync(List<Guid> ids,
                                                 DateTime lockoutEnd,
                                                 CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            var request = new LockoutManyUserAccountsRequest
            {
                UserIds = ids,
                LockoutEnd = lockoutEnd
            };

            await _client.Lockout2Async(request, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie zablokowano wybranych użytkowników");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> UnlockAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            var request = new UnlockUserAccountRequest { UserId = id };

            await _client.UnlockAsync(id, request, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie odblokowano użytkownika");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }

    public async Task<Response> UnlockManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        Response response;

        try
        {
            var request = new UnlockManyUserAccountsRequest { UserIds = ids };

            await _client.Unlock2Async(request, cancellationToken);
            response = base.GenerateSuccessResponse("Pomyślnie odblokowano wybranych użytkowników");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions(ex);
        }

        return response;
    }
}
