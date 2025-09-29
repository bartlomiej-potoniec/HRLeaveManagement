using HRLeaveManagement.Domain.BoundedEntities;
using HRLeaveManagement.Domain.Contracts;
using HRLeaveManagement.Domain.Entities;
using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Policies.Leaves;

public class AnnualLeavePolicy : ILeaveAllocationPolicy
{
    public string Code => "ANNUAL";

    private const int CONDITION_SENIORITY = 10;
    private const int LEAVE_DAYS_FOR_SENIORITY_FEWER_THAN_10 = 20;
    private const int LEAVE_DAYS_FOR_SENIORITY_GREATER_THAN_10 = 26;

    public int? CalculateDays(LeaveEvaluationContext context)
    {
        int fullySeniority;

        var educationSeniority = GetEducationSeniority(context.EmployeeEducations);
        var contractSeniority = GetContractSeniority(context.EmployeeContracts);
        var experienceSeniority = GetExperienceSeniority(context.EmployeeExperiences);

        fullySeniority = educationSeniority + contractSeniority + experienceSeniority;

        return fullySeniority < CONDITION_SENIORITY
            ? LEAVE_DAYS_FOR_SENIORITY_FEWER_THAN_10
            : LEAVE_DAYS_FOR_SENIORITY_GREATER_THAN_10;
    }

    public bool IsEligible(LeaveEvaluationContext context)
        => context.CurrentContract is not null && 
            context.CurrentContract.ContractType is ContractType.Employment;

    private static int GetEducationSeniority(IEnumerable<EmployeeEducation> educations)
    {
        var highestEducation = educations
            .Select(edu => edu.EducationType)
            .OrderByDescending(edu => edu)
            .FirstOrDefault();

        var seniority = highestEducation switch
        {
            EducationType.GeneralSecondary => 3,
            EducationType.Secondary        => 4,
            EducationType.PostSecondary    => 6,
            EducationType.Higher           => 8,
            _                              => 0
        };

        return seniority;
    }

    private static int GetContractSeniority(IEnumerable<EmployeeContract> contracts)
    {
        int fullYears = 0;

        foreach (var contract in contracts
            .Where(con => con.ContractType is ContractType.Employment))
        {
            var endDate = contract.ExpiredAt ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var years = endDate.Year - contract.StartedAt.Year;

            if (endDate < contract.StartedAt.AddYears(years))
            {
                years--;
            }

            fullYears += years;
        }

        return fullYears;
    }

    private static int GetExperienceSeniority(IEnumerable<EmployeeExperience> experiences)
    {
        int fullYears = 0;

        foreach (var contract in experiences
            .Where(con => con.ContractType is ContractType.Employment))
        {
            var endDate = contract.EmployedTo;
            var years = endDate.Year - contract.EmployedFrom.Year;

            if (endDate < contract.EmployedFrom.AddYears(years))
            {
                years--;
            }

            fullYears += years;
        }

        return fullYears;
    }
}
