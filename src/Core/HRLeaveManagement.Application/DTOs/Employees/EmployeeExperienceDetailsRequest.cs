using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Application.DTOs.Employees;

public sealed record EmployeeExperienceDetailsRequest(int? Id,
                                                      ContractType ContractType,
                                                      string PreviousCompanyName,
                                                      string Position,
                                                      DateTime EmployedFrom,
                                                      DateTime EmployedTo,
                                                      string? ExperienceDetails = null /* new */,
                                                      IEnumerable<EmployeeDocumentDetailsRequest> EmployeeDocuments = null);
