using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.Employee.Queries;
using MediatR;
using AutoMapper;

namespace HRLeaveManagement.Application.Features.Employee.QueryHandlers;

public sealed class GetEmployeeContractWithDetailsCommandHandler(IEmployeeRepository employeeRepository,
                                                                 IMapper mapper,
                                                                 IAppLogger<GetEmployeeContractWithDetailsCommandHandler> logger)
    : IRequestHandler<GetEmployeeContractWithDetailsCommand, EmployeeContractDetailsDTO>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetEmployeeContractWithDetailsCommandHandler> _logger = logger;

    public async Task<EmployeeContractDetailsDTO> Handle(GetEmployeeContractWithDetailsCommand request,
                                                         CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching employee contract with ID: {ContractId} for employee with ID: {EmployeeId} started", request.ContractId, request.EmployeeId);

        var employeeContract = await _employeeRepository
            .GetContractByIdAsync(request.ContractId, cancellationToken)
            ?? throw new NotFoundException($"No contract with ID: { request.ContractId } found");

        var employeeContractDto = _mapper.Map<EmployeeContractDetailsDTO>(employeeContract);

        _logger.LogInformation("Fetching employee contract with ID: {ContractId} for employee with ID: {EmployeeId} successful", request.ContractId, request.EmployeeId);

        return employeeContractDto;
    }
}
