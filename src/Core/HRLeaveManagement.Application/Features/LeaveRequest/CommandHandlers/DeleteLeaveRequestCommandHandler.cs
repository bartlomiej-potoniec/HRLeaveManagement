using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class DeleteLeaveRequestCommandHandler(ILeaveRequestRepository repository) 
    : IRequestHandler<DeleteLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _repository = repository;

    public async Task Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        await _repository.DeleteAsync(leaveRequest);
    }
}
