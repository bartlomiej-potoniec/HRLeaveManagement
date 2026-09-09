using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeContractRequest(ContractType ContractType,
                                      DateTime EmployeedFrom,
                                      DateTime? EmployeedTo = null,
                                      string? ContractDetails = null, /* new */
                                      IEnumerable<EmployeeDocumentRequest>? EmployeeDocuments = null /* new */);
