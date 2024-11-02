using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeExperience
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    public ContractType ContractType { get; private set; }

    public DateOnly EmployedFrom { get; private set; }
    public DateOnly EmployedTo { get; private set; }
    public int TotalEmployment { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private EmployeeExperience() {}


    // Factory Methods
    public static EmployeeExperience Create(Guid employeeId,
                                            ContractType contractType,
                                            DateOnly employedFrom,
                                            DateOnly employedTo)
        => new()
        {
            EmployeeId = employeeId,
            ContractType = contractType,
            EmployedFrom = employedFrom,
            EmployedTo = employedTo,
            TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(EmployeeExperience entity,
                              Guid employeeId,
                              ContractType contractType,
                              DateOnly employedFrom,
                              DateOnly employedTo)
    {
        entity.EmployeeId = employeeId;
        entity.ContractType = contractType;
        entity.EmployedFrom = employedFrom;
        entity.EmployedTo = employedTo;
        entity.TotalEmployment = employedTo.DayNumber - employedFrom.DayNumber;
        entity.ModifiedAt = DateTime.UtcNow;
    }
}
