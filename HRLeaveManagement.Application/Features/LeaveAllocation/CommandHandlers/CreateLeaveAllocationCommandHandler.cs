using DomainLeaveAllocation = HRLeaveManagement.Domain.LeaveAllocation;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Contracts.Identity;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.CommandHandlers;

public sealed class CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
                                                        ILeaveTypeRepository leaveTypeRepository,
                                                        IUserService userService,
                                                        IAppLogger<CreateLeaveAllocationCommand> logger) 
    : IRequestHandler<CreateLeaveAllocationCommand>
{
    private readonly ILeaveAllocationRepository _leaveAllocationRepository = leaveAllocationRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IUserService _userService = userService;
    private readonly IAppLogger<CreateLeaveAllocationCommand> _logger = logger;

    public async Task Handle(CreateLeaveAllocationCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("");
            throw new BadRequestException("Invalid leave allocation request", validationResult);
        }

        // Get Leave type for allocations
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId)
            ?? throw new NotFoundException($"No leave type with id { request.LeaveTypeId } found");

        var employees = await _userService.GetEmployees();
        var period = DateTime.Now.Year;

        // Assign Allocations only if an allocation doesn't already exist for period and leave type
        List<DomainLeaveAllocation> allocations = [];

        foreach (var employee in employees)
        {
            bool isAllocationExist = await _leaveAllocationRepository
                .IsAllocationExistAsync(employee.Id, request.LeaveTypeId, period);

            if (isAllocationExist) continue;

            allocations.Add(new DomainLeaveAllocation
            {
                EmployeeId = employee.Id,
                LeaveTypeId = request.LeaveTypeId,
                NumberOfDays = leaveType.DefaultDays,
                Period = period
            });
        }

        if (allocations.Any())
            await _leaveAllocationRepository.AddAllocationsAsync(allocations);
    }
}
