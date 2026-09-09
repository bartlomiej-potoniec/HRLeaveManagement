using HRLeaveManagement.Application.DTOs.Sections;
using AutoMapper;
using HRLeaveManagement.Domain.Department.Section;

namespace HRLeaveManagement.Application.MappingProfiles;

public class SectionProfile : Profile
{
    public SectionProfile()
    {
        CreateMap<Section, SectionDTO>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ReverseMap();

        CreateMap<Section, SectionDetailsDTO>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ReverseMap();
    }
}
