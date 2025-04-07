using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace HRLeaveManagement.BlazorUI.Handlers;

public sealed class JwtAuthorizationMessageHandler(ILocalStorageService localStorage) : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage = localStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("token", cancellationToken);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var result = await base.SendAsync(request, cancellationToken);
        return result;
    }
}
