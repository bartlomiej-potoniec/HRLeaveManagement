using AutoMapper;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.LeaveTypes;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Queries;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.QueryHandlers;

public sealed class GetLeaveTypeWithDetailsQueryHandler(ILeaveTypeRepository leaveTypeRepository,
                                                        IMapper mapper,
                                                        IAppLogger<GetLeaveTypeWithDetailsQueryHandler> logger)
    : IRequestHandler<GetLeaveTypeWithDetailsQuery, LeaveTypeDetailsDTO>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetLeaveTypeWithDetailsQueryHandler> _logger = logger;

    public async Task<LeaveTypeDetailsDTO> Handle(GetLeaveTypeWithDetailsQuery request,
                                                  CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching leave type with ID: {Id} started", request.Id);

        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"No leave type with ID: { request.Id } found");
        
        var leaveTypeDetailsDto = _mapper.Map<LeaveTypeDetailsDTO>(leaveType);

        _logger.LogInformation("Fetching leave type with ID: {Id} successful", request.Id);

        return leaveTypeDetailsDto;
    }
}
