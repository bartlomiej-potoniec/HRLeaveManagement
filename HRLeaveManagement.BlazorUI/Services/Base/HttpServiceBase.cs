using HRLeaveManagement.BlazorUI.Models;

namespace HRLeaveManagement.BlazorUI.Services.Base;

public class HttpServiceBase(IClient client)
{
    protected readonly IClient _client = client;

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex) 
        => ex.StatusCode switch
        {
            400 => new Response<Guid>() { Message = "Invalid data was submitted", ValidationErrors = ex.Response },
            404 => new Response<Guid>() { Message = "The record was not found" },
            _ => new Response<Guid>() { Message = "Something went wrong, please try again later" }
        }; 
}
