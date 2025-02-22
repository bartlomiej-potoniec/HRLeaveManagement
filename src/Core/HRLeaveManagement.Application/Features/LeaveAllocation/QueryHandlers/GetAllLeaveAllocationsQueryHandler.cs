using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.LeaveAllocations;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.QueryHandlers;

public sealed class GetAllLeaveAllocationsQueryHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                       IUserService userService,
                                                       IMapper mapper,
                                                       IAppLogger<GetAllLeaveAllocationsQueryHandler> logger)
    : IRequestHandler<GetAllLeaveAllocationsQuery, IEnumerable<LeaveAllocationDTO>>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllLeaveAllocationsQueryHandler> _logger = logger;

    public async Task<IEnumerable<LeaveAllocationDTO>> Handle(GetAllLeaveAllocationsQuery request,
                                                              CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all departments started");

        var users = await _userService.GetAllUsersInRoleAsync("Employee", cancellationToken);
        var leaveAllocations = await _leaveAllocationRepository.GetAllAsync(cancellationToken);

        var leaveAllocationDtos = leaveAllocations.Select(allocation =>
        {
            var user = users
                .FirstOrDefault(u => u.EmployeeId == allocation.EmployeeId)
                ?? throw new NotFoundException($"No user with employee ID: { allocation.EmployeeId } found");

            var leaveAllocationDto = _mapper.Map<LeaveAllocationDTO>(
                allocation, 
                opt => opt.Items["EmployeeName"] = $"{ user.FirstName } { user.LastName }"
            );

            return leaveAllocationDto;
        });

        _logger.LogInformation("Fetching all departments successful");

        return leaveAllocationDtos;
    }
}
