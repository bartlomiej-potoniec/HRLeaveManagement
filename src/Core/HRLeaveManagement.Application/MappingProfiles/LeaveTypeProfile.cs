using AutoMapper;
using HRLeaveManagement.Application.DTOs.LeaveTypes;
using HRLeaveManagement.Domain.Leave.LeaveType;
using HRLeaveManagement.Application.Features.LeaveType.CreateLeaveType.Commands;
using HRLeaveManagement.Application.Features.LeaveType.UpdateLeaveType.Commands;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveTypeProfile : Profile
{
    public LeaveTypeProfile()
    {
        CreateMap<LeaveType, LeaveTypeDTO>().ReverseMap();
        CreateMap<LeaveType, LeaveTypeDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveTypeCommand, LeaveType>();
        CreateMap<UpdateLeaveTypeCommand, LeaveType>();
    }
}