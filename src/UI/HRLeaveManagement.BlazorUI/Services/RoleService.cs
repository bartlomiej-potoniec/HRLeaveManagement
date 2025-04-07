using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Roles;
using HRLeaveManagement.BlazorUI.Models;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class RoleService(IClient client,
                                ILocalStorageService localStorage,
                                IMapper mapper)
    : HttpServiceBase(client, localStorage), IRoleService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response<List<RoleViewModel>>> GetAllAsync()
    {
        Response<List<RoleViewModel>> response;

        try
        {
            var roles = await _client.RolesAsync();
            var viewModel = _mapper.Map<List<RoleViewModel>>(roles);

            response = base.GenerateSuccessResponse("Roles correctly fetched", viewModel);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<List<RoleViewModel>>(ex);
        }

        return response;
    }
}
