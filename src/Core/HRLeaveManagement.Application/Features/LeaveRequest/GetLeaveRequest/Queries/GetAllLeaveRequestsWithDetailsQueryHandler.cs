using DomainLeaveRequest = HRLeaveManagement.Domain.Leave.LeaveRequest.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveRequest.GetLeaveRequest.Queries;

public sealed class GetAllLeaveRequestsWithDetailsQueryHandler(ILeaveRequestRepository repository,
                                                               IUserService userService,
                                                               IMapper mapper)
    : IRequestHandler<GetAllLeaveRequestsWithDetailsQuery, IEnumerable<LeaveRequestDTO>>
{
    private readonly ILeaveRequestRepository _repository = repository;
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<LeaveRequestDTO>> Handle(GetAllLeaveRequestsWithDetailsQuery request,
                                                           CancellationToken cancellationToken)
    {
        List<DomainLeaveRequest> leaveRequests = [];
        List<LeaveRequestDTO> requests = [];

        // TODO: Check if it is logged in employee
        if (_userService.IsUserLoggedIn)
        {
            var userId = _userService.UserId
                ?? throw new NotFoundException("No user found");

            leaveRequests = (await _repository
                .GetEmployeeLeaveRequestsWithDetailsAsync(Guid.Parse(userId), cancellationToken))
                .ToList();

            var employee = await _userService.GetUserByIdAsync(Guid.Parse(userId), cancellationToken);

            requests = _mapper.Map<List<LeaveRequestDTO>>(leaveRequests, opt =>
                opt.AfterMap((src, dest) => dest.Select(d => d with { Employee = employee }))
            );
        }

        else
        {
            leaveRequests = (await _repository
                .GetAllLeaveRequestsWithDetailsAsync(cancellationToken))
                .ToList();

            requests = _mapper
                .Map<List<LeaveRequestDTO>>(leaveRequests)
                .Select(async dto => 
                    dto with { Employee = 
                        await _userService.GetUserByIdAsync(Guid.Parse(dto.RequestingEmployeeId), cancellationToken) }
                )
                .Select(task => task.Result)
                .ToList();   
        }

        return requests;
    }
}
