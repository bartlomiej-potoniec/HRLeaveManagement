using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;

namespace HRLeaveManagement.Domain.Employee.ExternalContracts;

public sealed record EmployeeLeaveInformation(
    bool HasCurrentContract,
    ContractType? ContractType,
    GenderType Gender,
    EducationType? HighestEducation,
    int? YearsOfGeneralExperience,
    int? YearsOfCompanyExperience);