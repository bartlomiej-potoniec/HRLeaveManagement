using AutoMapper;
using DomainLeaveRequest = HRLeaveManagement.Domain.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries;
using MediatR;
using HRLeaveManagement.Application.Exceptions;

namespace HRLeaveManagement.Application.Features.LeaveRequest.QueryHandlers;

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
        var leaveRequests = new List<DomainLeaveRequest>();
        var requests = new List<LeaveRequestDTO>();

        // TODO: Check if it is logged in employee
        if (request.IsUserLoggedIn)
        {
            var userId = _userService.UserId
                ?? throw new NotFoundException("No user found");

            leaveRequests = 
                (List<DomainLeaveRequest>)await _repository.GetUserLeaveRequestsWithDetailsAsync(userId);

            var employee = await _userService.GetEmployee(userId);

            requests = _mapper.Map<List<LeaveRequestDTO>>(leaveRequests, opt =>
                opt.AfterMap((src, dest) => dest.Select(d => d with { Employee = employee }))
            );
        }

        else
        {
            leaveRequests = 
                (List<DomainLeaveRequest>)await _repository.GetAllLeaveRequestsWithDetailsAsync();

            requests = _mapper
                .Map<List<LeaveRequestDTO>>(leaveRequests)
                .Select(async dto => dto with { Employee = await _userService.GetEmployee(dto.RequestingEmployeeId) })
                .Select(task => task.Result)
                .ToList();   
        }

        return requests;
    }
}
