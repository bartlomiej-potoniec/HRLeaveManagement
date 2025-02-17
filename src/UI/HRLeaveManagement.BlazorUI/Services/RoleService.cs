using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Roles;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class RoleService(IClient client,
                                ILocalStorageService localStorage,
                                IMapper mapper)
    : HttpServiceBase(client, localStorage), IRoleService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<RoleViewModel>> GetAllAsync()
    {
        var roles = await _client.RolesAsync();
        var viewModel = _mapper.Map<List<RoleViewModel>>(roles);

        return viewModel;
    }
}
