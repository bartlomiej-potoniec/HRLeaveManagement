using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;
using HRLeaveManagement.BlazorUI.Models;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class SectionService(IClient client,
                                   ILocalStorageService localStorage,
                                   IMapper mapper)
    : HttpServiceBase(client, localStorage), ISectionService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response<List<SectionViewModel>>> GetAllAsync()
    {
        Response<List<SectionViewModel>> response;

        try
        {
            var sections = await _client.SectionsAllAsync();
            var viewModel = _mapper.Map<List<SectionViewModel>>(sections);

            response = base.GenerateSuccessResponse("GET operation success", viewModel);
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<List<SectionViewModel>>(ex);
        }

        return response;
    }
}
