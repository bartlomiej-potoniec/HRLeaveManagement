using AutoMapper;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDTO>().ReverseMap();
        CreateMap<LeaveRequest, LeaveRequestDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
        CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
    }
}
