using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Roles;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDTO, RoleViewModel>().ReverseMap();
    }
}
