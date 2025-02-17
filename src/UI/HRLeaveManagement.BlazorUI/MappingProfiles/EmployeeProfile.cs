using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public sealed class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDTO, EmployeeViewModel>().ReverseMap();
        CreateMap<EmployeeDetailsDTO, EmployeeViewModel>().ReverseMap();
        CreateMap<EmployeeContractDetailsDTO, EmployeeContractViewModel>().ReverseMap();
        CreateMap<EmployeeContractResponse, EmployeeContractViewModel>().ReverseMap();

        CreateMap<EmployeeContractRequest, EmployeeContractViewModel>().ReverseMap();
        CreateMap<EmployeeEducationRequest, EmployeeEducationViewModel>().ReverseMap();
        CreateMap<EmployeeExperienceRequest, EmployeeExperienceViewModel>().ReverseMap();

        CreateMap<EmployeeDetailsDTO, EmployeeDetailsViewModel>().ReverseMap();
        CreateMap<EmployeeDetailsDTO, CreateEmployeeDetailsViewModel>().ReverseMap();

        CreateMap<EmployeeDetailsViewModel, EditEmployeeDetailsViewModel>().ReverseMap();
        CreateMap<EmployeeContractDetailsViewModel, EmployeeContractViewModel>()
            .ForMember(dest => dest.EmployedFrom, src => src.MapFrom(opt => opt.StartedAt))
            .ForMember(dest => dest.EmployedTo, src => src.MapFrom(opt => opt.ExpiredAt))
            .ReverseMap();

        CreateMap<EmployeeEducationDetailsViewModel, EmployeeEducationViewModel>().ReverseMap();
        CreateMap<EmployeeExperienceDetailsViewModel, EmployeeExperienceViewModel>().ReverseMap();


        CreateMap<CreateEmployeeDetailsViewModel, CreateEmployeeWithDetailsCommand>()
            .ForMember(dest => dest.EmployeeContract, src => src.MapFrom(opt => opt.Contract))
            .ForMember(dest => dest.EmployeeEducations, src => src.MapFrom(opt => opt.Educations))
            .ForMember(dest => dest.EmployeeExperiences, src => src.MapFrom(opt => opt.Experiences))
            .ReverseMap();

        CreateMap<EmployeeContractDetailsDTO, EmployeeContractDetailsViewModel>().ReverseMap();
        CreateMap<EmployeeEducationDetailsDTO, EmployeeEducationDetailsViewModel>().ReverseMap();
        CreateMap<EmployeeExperienceDetailsDTO, EmployeeExperienceDetailsViewModel>().ReverseMap();

        CreateMap<EditEmployeeDetailsViewModel, UpdateEmployeeWithDetailsCommand>()
            .ForMember(dest => dest.Id, src => src.MapFrom(opt => opt.EmployeeId))
            .ForMember(dest => dest.EmployeeContracts, src => src.MapFrom(opt => opt.Contracts))
            .ForMember(dest => dest.EmployeeEducations, src => src.MapFrom(opt => opt.Educations))
            .ForMember(dest => dest.EmployeeExperiences, src => src.MapFrom(opt => opt.Experiences))
            .ReverseMap();

        CreateMap<EmployeeContractViewModel, EmployeeContractDetailsRequest>().ReverseMap();
        CreateMap<EmployeeEducationViewModel, EmployeeEducationDetailsRequest>().ReverseMap();
        CreateMap<EmployeeExperienceViewModel, EmployeeExperienceDetailsRequest>().ReverseMap();
    }
}
