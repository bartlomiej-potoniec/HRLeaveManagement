using LeaveRequestEntity = HRLeaveManagement.Domain.Leave.LeaveRequest.LeaveRequest;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Validation;
using MediatR;
using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Leave.LeaveRequest.CheckerContracts;

namespace HRLeaveManagement.Application.Features.LeaveRequest.CreateLeaveRequest.Commands;

public sealed class CreateLeaveRequestCommandHandler(ILeaveTypeRepository leaveTypeRepository,
                                                     IEmployeeRepository employeeRepository,
                                                     ILeaveRequestApproverSuperiorOfEmployeeChecker leaveRequestRuleSet,
                                                     IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
                                                     IEmployeeContextFactory employeeContextFactory,
                                                     IUserService userService,
                                                     IUnitOfWork unitOfWork,
                                                     TimeProvider timeProvider,
                                                     IAppLogger<CreateLeaveRequestCommand> logger)
    : IRequestHandler<CreateLeaveRequestCommand, int>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository = leaveTypeRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly ILeaveRequestApproverSuperiorOfEmployeeChecker _leaveRequestRuleSet = leaveRequestRuleSet;
    private readonly IEmployeeDocumentNumberUniqueChecker _employeeDocumentRuleSet = employeeDocumentRuleSet;
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly IUserService _userService = userService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IAppLogger<CreateLeaveRequestCommand> _logger = logger;

    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveRequestCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation error occurred while proccessing {Command}", nameof(CreateLeaveRequestCommand));
            throw new BadRequestException("Invalid leave request", validationResult);
        }

        var requestingEmployeeId = _userService.EmployeeId;
        var todaysDate = _timeProvider.GetUtcNow().DateTime;
        var currentYear = todaysDate.Year;

        var employee = await _employeeRepository
            .GetWithLeaveRequestsAndAllocationByIdAsync(requestingEmployeeId, request.LeaveTypeId, currentYear, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { requestingEmployeeId } " +
                $"and allocation for leave type ID: { request.LeaveTypeId } and current year found");

        var approver = await _employeeRepository
            .GetByIdAsync(request.ApproverId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { requestingEmployeeId } found");

        var substitutor = await _employeeRepository
            .GetByIdAsync(request.SubstitutorId, cancellationToken)
            ?? throw new NotFoundException($"No employee with ID: { requestingEmployeeId } found");

        var leaveType = await _leaveTypeRepository
            .GetByIdAsync(request.LeaveTypeId, cancellationToken)
            ?? throw new NotFoundException($"No leave typ with ID: { request.LeaveTypeId } found");

        var employeeWithLeaveRequestsAndAllocation = _employeeContextFactory.AsEmployeeWithLeaveRequestsAndAllocation(employee);
        var leaveAllocation = employeeWithLeaveRequestsAndAllocation.LeaveAllocation;

        var documentPayloads = (request.EmployeeDocuments ?? [])
            .Select(doc => new EmployeeDocumentPayload(
                doc.Title,
                doc.DocumentNumber,
                doc.FileUrl,
                doc.Description
            ));

        var documents = await EmployeeDocument.CreateManyAsync(_employeeDocumentRuleSet, documentPayloads, cancellationToken);

        var leaveRequest = await LeaveRequestEntity.CreateAsync(
            _leaveRequestRuleSet,
            employeeWithLeaveRequestsAndAllocation,
            leaveType,
            DateOnly.FromDateTime(request.StartedAt),
            DateOnly.FromDateTime(request.EndedAt),
            approver,
            substitutor,
            request.RequesterComment,
            request.ReasonDescription,
            documents,
            cancellationToken
        );

        employeeWithLeaveRequestsAndAllocation.AddLeaveRequest(leaveRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return leaveRequest.Id;
    }
}
