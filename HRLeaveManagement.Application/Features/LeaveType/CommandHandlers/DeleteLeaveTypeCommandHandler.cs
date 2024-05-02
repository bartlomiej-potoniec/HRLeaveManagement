﻿using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class DeleteLeaveTypeCommandHandler(ILeaveTypeRepository repository) 
    : IRequestHandler<DeleteLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _repository = repository;

    public async Task Handle(DeleteLeaveTypeCommand request,
                             CancellationToken cancellationToken)
    {
        var leaveType = await _repository.GetByIdAsync(request.Id)
            ?? throw new Exception($"No leave type with id: { request.Id }");

        await _repository.DeleteAsync(leaveType);
    }
}