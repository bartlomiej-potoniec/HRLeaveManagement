using HRLeaveManagement.BlazorUI.Models;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace HRLeaveManagement.BlazorUI.Services.Base;

public class HttpServiceBase(IClient client, ILocalStorageService localStorage)
{
    protected readonly IClient _client = client;
    protected readonly ILocalStorageService _localStorage = localStorage;

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex) 
        => ex.StatusCode switch
        {
            400 => new Response<Guid>() { Message = "Invalid data was submitted", ValidationErrors = ex.Response },
            404 => new Response<Guid>() { Message = "The record was not found" },
            _ => new Response<Guid>() { Message = "Something went wrong, please try again later" }
        }; 

    protected async Task AddBearerToken()
    {
        var isTokenPresent = await _localStorage.ContainKeyAsync("token");

        if (!isTokenPresent) return;

        var token = await _localStorage.GetItemAsync<string>("token");

        _client.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
}
