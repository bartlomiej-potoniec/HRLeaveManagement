using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractDetailsRequest(int? Id,
                                             ContractType ContractType,
                                             DateTime StartedAt,
                                             DateTime? ExpiredAt,
                                             string? ContractDetails, /* new */
                                             IEnumerable<EmployeeDocumentDetailsRequest> EmployeeDocuments /* new */);