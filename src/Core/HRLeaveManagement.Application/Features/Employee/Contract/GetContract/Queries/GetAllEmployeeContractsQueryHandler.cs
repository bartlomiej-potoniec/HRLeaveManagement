using HRLeaveManagement.Application.Contracts.Infrastructure.Logging;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Application.Contracts.Persistence.Repositories;
using AutoMapper;
using MediatR;

namespace HRLeaveManagement.Application.Features.Employee.Contract.GetContract.Queries;

public sealed class GetAllEmployeeContractsQueryHandler(IEmployeeRepository employeeRepository,
                                                        IMapper mapper,
                                                        IAppLogger<GetAllEmployeeContractsQueryHandler> logger)
    : IRequestHandler<GetAllEmployeeContractsQuery, IEnumerable<EmployeeContractDetailsDTO>>
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IAppLogger<GetAllEmployeeContractsQueryHandler> _logger = logger;

    public async Task<IEnumerable<EmployeeContractDetailsDTO>> Handle(GetAllEmployeeContractsQuery request,
                                                                      CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all employee contracts for employee with ID: {Id} started", request.EmployeeId);

        var employeeContracts = await _employeeRepository.GetAllContractsByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        var employeeContractDtos = _mapper.Map<IEnumerable<EmployeeContractDetailsDTO>>(employeeContracts);

        _logger.LogInformation("Fetching all employee contracts for employee with ID: {Id} successful", request.EmployeeId);

        return employeeContractDtos;
    }
}
