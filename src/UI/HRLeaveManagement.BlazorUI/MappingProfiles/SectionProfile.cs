using AutoMapper;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Sections;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public class SectionProfile : Profile
{
    public SectionProfile()
    {
        CreateMap<SectionDTO, SectionViewModel>().ReverseMap();
    }
}
