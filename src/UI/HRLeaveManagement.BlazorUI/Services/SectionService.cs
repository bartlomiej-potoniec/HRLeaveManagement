using AutoMapper;
using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class SectionService(IClient client,
                                   ILocalStorageService localStorage,
                                   IMapper mapper)
    : HttpServiceBase(client, localStorage), ISectionService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<SectionViewModel>> GetAllAsync()
    {
        var sections = await _client.SectionsAllAsync();
        var viewModel = _mapper.Map<List<SectionViewModel>>(sections);

        return viewModel;
    }
}
