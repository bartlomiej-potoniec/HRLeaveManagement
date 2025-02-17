using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Application.DTOs.Employees;

public sealed record EmployeeExperienceDetailsRequest(int? Id,
                                                      ContractType ContractType,
                                                      string PreviousCompanyName,
                                                      string Position,
                                                      DateTime EmployedFrom,
                                                      DateTime EmployedTo);
