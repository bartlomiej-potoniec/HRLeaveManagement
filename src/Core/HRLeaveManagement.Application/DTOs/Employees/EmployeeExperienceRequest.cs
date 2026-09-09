using HRLeaveManagement.Domain.Employee.Contract;

namespace HRLeaveManagement.Application.DTOs.Employees;

public record EmployeeExperienceRequest(ContractType ContractType,
                                        string PreviousCompanyName,
                                        string Position,
                                        DateTime EmployedFrom,
                                        DateTime EmployedTo,
                                        string? ExperienceDetails = null, /* new */
                                        IEnumerable<EmployeeDocumentRequest>? EmployeeDocuments = null /* new */);
