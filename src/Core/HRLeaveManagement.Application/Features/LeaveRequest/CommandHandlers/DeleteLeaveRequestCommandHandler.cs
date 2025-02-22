using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class DeleteLeaveRequestCommandHandler(ILeaveRequestRepository repository) 
    : IRequestHandler<DeleteLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _repository = repository;

    public async Task Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);

        await _repository.DeleteAsync(leaveRequest, cancellationToken);
    }
}
