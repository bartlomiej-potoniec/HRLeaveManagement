using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Domain;
using AutoMapper;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveType, LeaveTypeDTO>().ReverseMap();
        CreateMap<LeaveType, LeaveTypeDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveTypeCommand, LeaveType>();
        CreateMap<UpdateLeaveTypeCommand, LeaveType>();

        CreateMap<LeaveAllocation, LeaveAllocationDTO>().ReverseMap();
    }
}