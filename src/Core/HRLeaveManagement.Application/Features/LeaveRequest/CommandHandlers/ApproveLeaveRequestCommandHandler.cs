using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands;
using HRLeaveManagement.Application.Validation;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CommandHandlers;

public sealed class ApproveLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                      IEmployeeRepository employeeRepository,
                                                      IUserService userService,
                                                      IUnitOfWork unitOfWork,
                                                      IAppLogger<ApproveLeaveRequestCommandHandler> logger)
    : IRequestHandler<ApproveLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository = leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAppLogger<ApproveLeaveRequestCommandHandler> _logger = logger;

    public async Task Handle(ApproveLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new ApproveLeaveRequestCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(ApproveLeaveRequestCommand));
            throw new BadRequestException("Invalid cancelation leave request", validationResult);
        }

        var requestingEmployeeId = _userService.EmployeeId;

        var employee = await _employeeRepository
            .GetByIdAsync(requestingEmployeeId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { requestingEmployeeId } found");

        var leaveRequest = await _leaveRequestRepository
            .GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"No leave request with ID: { request.Id } found");

        leaveRequest.Approve(employee);

        _logger.LogInformation("Approving for leave request with ID: {RequestId} by employee with ID: {EmployeeId}", request.Id, requestingEmployeeId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
