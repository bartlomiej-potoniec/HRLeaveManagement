using AutoMapper;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public sealed class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequestDTO, LeaveRequestViewModel>()
            .ReverseMap();

        CreateMap<LeaveRequestDetailsDTO, LeaveRequestViewModel>()
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
            .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => src.LeaveType))
            .ReverseMap();

        CreateMap<LeaveRequestViewModel, CreateLeaveRequestCommand>();

        CreateMap<Employee, EmployeeViewModel>()
            .ReverseMap();
    }
}
