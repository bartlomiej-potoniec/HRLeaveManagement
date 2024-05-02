using AutoMapper;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveType, LeaveTypeDTO>().ReverseMap();
        CreateMap<LeaveType, LeaveTypeDetailsDTO>().ReverseMap();
    }
}
