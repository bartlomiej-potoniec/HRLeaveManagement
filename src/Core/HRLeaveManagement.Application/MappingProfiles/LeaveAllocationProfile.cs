using HRLeaveManagement.Application.DTOs;
using AutoMapper;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Domain.Entities;

namespace HRLeaveManagement.Application.MappingProfiles;

public class LeaveAllocationProfile : Profile
{
    public LeaveAllocationProfile()
    {
        CreateMap<LeaveAllocation, LeaveAllocationDTO>().ReverseMap();
        CreateMap<LeaveAllocation, LeaveAllocationDetailsDTO>().ReverseMap();

        CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
        CreateMap<UpdateLeaveAllocationCommand, LeaveAllocation>();
    }
}
