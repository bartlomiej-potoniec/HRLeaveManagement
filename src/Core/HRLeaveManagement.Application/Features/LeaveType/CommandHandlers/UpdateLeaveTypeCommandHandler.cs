using AutoMapper;
using DomainLeaveType = HRLeaveManagement.Domain.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using MediatR;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Exceptions;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class UpdateLeaveTypeCommandHandler(ILeaveTypeRepository repository,
                                                  IAppLogger<UpdateLeaveTypeCommandHandler> logger,
                                                  IMapper mapper)
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(UpdateLeaveTypeCommand request,
                             CancellationToken cancellationToken)
    {
        var validator = new UpdateLeaveTypeCommandValidator(_repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(LeaveType), request.Id);
            throw new BadRequestException("Invalid leave type", validationResult);
        }

        var leaveType = _mapper.Map<DomainLeaveType>(request);

        await _repository.UpdateAsync(leaveType);
    }
}
