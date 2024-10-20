using AutoMapper;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

namespace HRLeaveManagement.BlazorUI.MappingProfiles;

public sealed class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveTypeDTO, LeaveTypeViewModel>().ReverseMap();
        CreateMap<LeaveTypeDetailsDTO, LeaveTypeViewModel>().ReverseMap();

        CreateMap<LeaveTypeViewModel, CreateLeaveTypeCommand>();
        CreateMap<LeaveTypeViewModel, UpdateLeaveTypeCommand>();
    }
}
