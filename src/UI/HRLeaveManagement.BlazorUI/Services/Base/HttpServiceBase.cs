using HRLeaveManagement.BlazorUI.Models;
using Blazored.LocalStorage;

namespace HRLeaveManagement.BlazorUI.Services.Base;

public class HttpServiceBase(IClient client, ILocalStorageService localStorage)
{
    protected readonly IClient _client = client;
    protected readonly ILocalStorageService _localStorage = localStorage;

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex) 
        => ex.StatusCode switch
        {
            400 => new Response<Guid>() { Message = "Invalid data was submitted", IsSuccess = false },
            404 => new Response<Guid>() { Message = "The record was not found", IsSuccess = false },
            _ => new Response<Guid>() { Message = "Something went wrong, please try again later", IsSuccess = false }
        };

    protected Response<Guid> GenerateSuccessResponse(string message)
        => new()
        {
            Message = message,
            IsSuccess = true
        };
}
