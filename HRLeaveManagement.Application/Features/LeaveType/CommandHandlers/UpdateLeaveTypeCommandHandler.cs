using AutoMapper;
using DomainLeaveType = HRLeaveManagement.Domain.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository repository, IMapper mapper)
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(UpdateLeaveTypeCommand request,
                             CancellationToken cancellationToken)
    {
        var leaveType = _mapper.Map<DomainLeaveType>(request);
        leaveType.ModifiedAt = DateTime.UtcNow;

        var result = await _repository.UpdateAsync(leaveType)
            ?? throw new Exception();
    }
}
