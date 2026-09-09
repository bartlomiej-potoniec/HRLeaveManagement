using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.ExternalContracts;

namespace HRLeaveManagement.Domain.Leave.Policies;

public class AnnualLeavePolicy : ILeavePolicy
{
    public string Code => "ANNUAL";

    private const int ConditionSeniority = 10;
    private const int LeaveDaysForSenirityFewerThan10 = 20;
    private const int LeaveDaysForSeniorityGreaterThan10 = 26;

    public int? CalculateDaysFor(EmployeeLeaveInformation employee)
    {
        int fullySeniority;

        var educationSeniority = GetEducationSeniority(employee.HighestEducation);
        var contractSeniority = employee.YearsOfCompanyExperience ?? 0;
        var experienceSeniority = employee.YearsOfGeneralExperience ?? 0;

        fullySeniority = educationSeniority + contractSeniority + experienceSeniority;
        return fullySeniority < ConditionSeniority
            ? LeaveDaysForSenirityFewerThan10
            : LeaveDaysForSeniorityGreaterThan10;
    }

    public bool IsEligibleFor(EmployeeLeaveInformation employee)
        => employee.HasCurrentContract && employee.ContractType is ContractType.Employment;

    private static int GetEducationSeniority(EducationType? highestEducation) => highestEducation switch
    {
        EducationType.GeneralSecondary => 3,
        EducationType.Secondary => 4,
        EducationType.PostSecondary => 6,
        EducationType.Higher => 8,
        _ => 0
    };
}
