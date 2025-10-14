using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using AutoMapper;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDTO>()
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<LeaveRequest, LeaveRequestDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveRequestCommand, LeaveRequest>()
            .ForMember(dest => dest.RequestingEmployeeId, opt => opt.Ignore());
            //.ForMember(dest => dest.RequestedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
