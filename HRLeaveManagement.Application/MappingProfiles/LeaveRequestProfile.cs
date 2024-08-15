using AutoMapper;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDTO>()
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        CreateMap<LeaveRequest, LeaveRequestDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveRequestCommand, LeaveRequest>()
            .ForMember(dest => dest.RequestingEmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.RequestedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
    }
}
