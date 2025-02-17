using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Users;
using HRLeaveManagement.BlazorUI.ViewModels;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDTO, UserViewModel>().ReverseMap();

        CreateMap<UserDetailsDTO, UserDetailsViewModel>()
            .ForMember(dest => dest.ShortId, opt => opt.MapFrom(src => src.Id.ToString().Substring(0, 8)))
            .ReverseMap();

        CreateMap<UserDetailsDTOPagedResult, PagedViewModel<UserDetailsViewModel>>().ReverseMap();
    }
}
