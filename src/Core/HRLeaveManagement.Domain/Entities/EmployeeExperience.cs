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
        => new()
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

    public static EmployeeExperience Create(Employee employee,
                                            ContractType contractType,
                                            string previousCompanyName,
                                            string position,
                                            DateTime employedFrom,
                                            DateTime employedTo)
        => new()
        {
            Employee = employee,
            ContractType = contractType,
            PreviousCompanyName = previousCompanyName,
            Position = position,
            EmployedFrom = DateOnly.FromDateTime(employedFrom),
            EmployedTo = DateOnly.FromDateTime(employedTo),
            TotalEmployment =  (employedTo - employedFrom).Days,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static EmployeeExperience Create(Guid employeeId,
                                            ContractType contractType,
                                            string previousCompanyName,
                                            string position,
                                            DateOnly employedFrom,
                                            DateOnly employedTo)
        => new()
        {
            EmployeeId = employeeId,
            ContractType = contractType,
            PreviousCompanyName = previousCompanyName,
            Position = position,
            EmployedFrom = employedFrom,
            EmployedTo = employedTo,
            TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(EmployeeExperience entity,
                              ContractType contractType,
                              string previousCompanyName,
                              string position,
                              DateOnly employedFrom,
                              DateOnly employedTo)
    {
        entity.ContractType = contractType;
        entity.PreviousCompanyName = previousCompanyName;
        entity.Position = position;
        entity.EmployedFrom = employedFrom;
        entity.EmployedTo = employedTo;
        entity.TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void Update(EmployeeExperience entity,
                              ContractType contractType,
                              string previousCompanyName,
                              string position,
                              DateTime employedFrom,
                              DateTime employedTo)
    {
        entity.ContractType = contractType;
        entity.PreviousCompanyName = previousCompanyName;
        entity.Position = position;
        entity.EmployedFrom = DateOnly.FromDateTime(employedFrom);
        entity.EmployedTo = DateOnly.FromDateTime(employedTo);
        entity.TotalEmployment = (employedTo - employedFrom).Days;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    #endregion
}
