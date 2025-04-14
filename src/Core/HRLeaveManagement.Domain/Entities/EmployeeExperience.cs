using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeExperience
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public ContractType ContractType { get; private set; }

    public string PreviousCompanyName { get; private set; }
    public string Position { get; private set; }
    public DateOnly EmployedFrom { get; private set; }
    public DateOnly EmployedTo { get; private set; }
    public int TotalEmployment { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private EmployeeExperience() {}


    #region Domain_Factory_Methods

    public static EmployeeExperience Create(Employee employee,
                                            ContractType contractType,
                                            string previousCompanyName,
                                            string position,
                                            DateOnly employedFrom,
                                            DateOnly employedTo)
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(contractType, previousCompanyName, position, employedFrom, employedTo);

        return new()
        {
            Employee = employee,
            ContractType = contractType,
            PreviousCompanyName = previousCompanyName,
            Position = position,
            EmployedFrom = employedFrom,
            EmployedTo = employedTo,
            TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public static EmployeeExperience Create(Employee employee,
                                            ContractType contractType,
                                            string previousCompanyName,
                                            string position,
                                            DateTime employedFrom,
                                            DateTime employedTo)
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        ValidateBaseRules(contractType, previousCompanyName, position, employedFrom, employedTo);

        return new()
        {
            Employee = employee,
            ContractType = contractType,
            PreviousCompanyName = previousCompanyName,
            Position = position,
            EmployedFrom = DateOnly.FromDateTime(employedFrom),
            EmployedTo = DateOnly.FromDateTime(employedTo),
            TotalEmployment = (employedTo - employedFrom).Days,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public static void Update(EmployeeExperience employeeExperience,
                              ContractType contractType,
                              string previousCompanyName,
                              string position,
                              DateOnly employedFrom,
                              DateOnly employedTo)
    {
        if (employeeExperience is null)
        {
            throw new ArgumentException("Employee experience must be included");
        }

        ValidateBaseRules(contractType, previousCompanyName, position, employedFrom, employedTo);

        employeeExperience.ContractType = contractType;
        employeeExperience.PreviousCompanyName = previousCompanyName;
        employeeExperience.Position = position;
        employeeExperience.EmployedFrom = employedFrom;
        employeeExperience.EmployedTo = employedTo;
        employeeExperience.TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber;
        employeeExperience.ModifiedAt = DateTime.UtcNow;
    }

    public static void Update(EmployeeExperience employeeExperience,
                              ContractType contractType,
                              string previousCompanyName,
                              string position,
                              DateTime employedFrom,
                              DateTime employedTo)
    {
        if (employeeExperience is null)
        {
            throw new ArgumentException("Employee experience must be included");
        }

        ValidateBaseRules(contractType, previousCompanyName, position, employedFrom, employedTo);

        employeeExperience.ContractType = contractType;
        employeeExperience.PreviousCompanyName = previousCompanyName;
        employeeExperience.Position = position;
        employeeExperience.EmployedFrom = DateOnly.FromDateTime(employedFrom);
        employeeExperience.EmployedTo = DateOnly.FromDateTime(employedTo);
        employeeExperience.TotalEmployment = (employedTo - employedFrom).Days;
        employeeExperience.ModifiedAt = DateTime.UtcNow;
    }

    private static void ValidateBaseRules(ContractType contractType,
                                          string previousCompanyName,
                                          string position,
                                          DateOnly employedFrom,
                                          DateOnly employedTo)
    {
        if (string.IsNullOrEmpty(previousCompanyName))
        {
            throw new ArgumentException("Previous company's name of employee cannot be empty");
        }

        if (string.IsNullOrEmpty(position))
        {
            throw new ArgumentException("Position at previous company of employee cannot be empty");
        }

        if (employedTo < employedFrom)
        {
            throw new InvalidOperationException("Employment end date at previous company must be greater than start date");
        }
    }

    private static void ValidateBaseRules(ContractType contractType,
                                          string previousCompanyName,
                                          string position,
                                          DateTime employedFrom,
                                          DateTime employedTo)
    {
        if (string.IsNullOrEmpty(previousCompanyName))
        {
            throw new ArgumentException("Previous company's name of employee cannot be empty");
        }

        if (string.IsNullOrEmpty(position))
        {
            throw new ArgumentException("Position at previous company of employee cannot be empty");
        }

        if (employedTo < employedFrom)
        {
            throw new InvalidOperationException("Employment end date at previous company must be greater than start date");
        }
    }

    #endregion
}
