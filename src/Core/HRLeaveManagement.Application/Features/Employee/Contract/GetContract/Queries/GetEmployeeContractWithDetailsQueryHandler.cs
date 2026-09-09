using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.GetContract.Queries;

public sealed class GetEmployeeContractWithDetailsQueryHandler(IEmployeeRepository employeeRepository,
                                                               IMapper mapper,
                                                               IAppLogger<GetEmployeeContractWithDetailsQueryHandler> logger)
    : IRequestHandler<GetEmployeeContractWithDetailsQuery, EmployeeContractDetailsDTO>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetEmployeeContractWithDetailsQueryHandler> _logger = logger;

    public async Task<EmployeeContractDetailsDTO> Handle(GetEmployeeContractWithDetailsQuery request,
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
