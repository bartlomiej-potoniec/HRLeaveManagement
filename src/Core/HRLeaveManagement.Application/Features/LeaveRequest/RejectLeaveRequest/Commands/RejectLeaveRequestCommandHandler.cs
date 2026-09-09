using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.RejectLeaveRequest.Commands;

public sealed class RejectLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                     IEmployeeRepository employeeRepository,
                                                     IUserService userService,
                                                     IUnitOfWork unitOfWork,
                                                     IAppLogger<RejectLeaveRequestCommandHandler> logger)
    : IRequestHandler<RejectLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<RejectLeaveRequestCommandHandler> _logger = logger;

    public async Task Handle(RejectLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new RejectLeaveRequestCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(RejectLeaveRequestCommand));
            throw new BadRequestException("Invalid rejection leave request", validationResult);
        }

        var requestingEmployeeId = _userService.EmployeeId;

        var employee = await _employeeRepository
            .GetByIdAsync(requestingEmployeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { requestingEmployeeId } found");

        var leaveRequest = await _leaveRequestRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No leave request with ID: { request.Id } found");

        leaveRequest.Reject(employee);

        _logger.LogInformation("Rejecting for leave request with ID: {RequestId} by employee with ID: {EmployeeId}", request.Id, requestingEmployeeId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
