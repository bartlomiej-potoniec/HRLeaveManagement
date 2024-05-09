using AutoMapper;
using DomainLeaveType = HRLeaveManagement.Domain.LeaveType;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands;
using HRLeaveManagement.Application.Validation;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.CommandHandlers;

public sealed class CreateLeaveTypeCommandHandler(ILeaveTypeRepository repository,
                                                  IMapper mapper,
                                                  IAppLogger<CreateLeaveTypeCommandHandler> logger)
    : IRequestHandler<CreateLeaveTypeCommand, int>
{
    private readonly ILeaveTypeRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger = logger;
 
    public async Task<int> Handle(CreateLeaveTypeCommand request,
                                  CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveTypeCommandValidator(_repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors in create request for {0}", nameof(LeaveType));
            throw new BadRequestException("Invalid Leave type", validationResult);
        }

        var leaveType = _mapper.Map<DomainLeaveType>(request);

        leaveType.CreatedAt = DateTime.UtcNow;
        leaveType.ModifiedAt = DateTime.UtcNow;

        var resultId = await _repository.CreateAsync(leaveType);

        return resultId;
    }
}
