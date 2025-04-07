using HRLeaveManagement.BlazorUI.Models;
using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Services.Base;

public class HttpServiceBase(IClient client, ILocalStorageService localStorage)
{
    protected readonly IClient _client = client;
    protected readonly ILocalStorageService _localStorage = localStorage;

    protected Response ConvertApiExceptions(ApiException ex)
        => ex.StatusCode switch
        {
            400 => new Response() { StatusCode = 400, Message = ex.Message, IsSuccess = false },
            404 => new Response() { StatusCode = 404, Message = "The record was not found", IsSuccess = false },
            _ => new Response() {StatusCode = null, Message = "Something went wrong, please try again later", IsSuccess = false }
        };

    protected Response<TData> ConvertApiExceptions<TData>(ApiException ex) 
        //where TData : class
            => ex.StatusCode switch
            {
                400 => new Response<TData>() { StatusCode = 400, Message = ex.Message, IsSuccess = false },
                404 => new Response<TData>() { StatusCode = 404, Message = "The record was not found", IsSuccess = false },
                _ => new Response<TData>() { StatusCode = null, Message = "Something went wrong, please try again later", IsSuccess = false }
            };

    protected Response GenerateSuccessResponse(string message)
        => new()
        {
            Message = message,
            IsSuccess = true
        };

    protected Response<TData> GenerateSuccessResponse<TData>(string message, TData data)
        where TData : class
            => new()
            {
                Data = data,
                Message = message,
                IsSuccess = true
            };
}
