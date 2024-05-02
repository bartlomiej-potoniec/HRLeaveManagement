using AutoMapper;
using DomainLeaveType = HRLeaveManagement.Domain.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class CreateLeaveTypeCommandHandler(ILeaveTypeRepository repository, IMapper mapper)
    : IRequestHandler<CreateLeaveTypeCommand, int>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
 
    public async Task<int> Handle(CreateLeaveTypeCommand request,
                                  CancellationToken cancellationToken)
    {
        var leaveType = _mapper.Map<DomainLeaveType>(request);

        leaveType.CreatedAt = DateTime.UtcNow;
        leaveType.ModifiedAt = DateTime.UtcNow;

        var result = await _repository.CreateAsync(leaveType)
            ?? throw new Exception();
            
        return result.Id;
    }
}
