using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeContract
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; set; }

    public ContractType ContractType { get; private set; }

    public DateOnly StartedAt { get; private set; }
    public DateOnly? ExpiredAt { get; private set; }
    public int? TotalDuration { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private EmployeeContract() {}


    #region Domain_Factory_Methods

    public static EmployeeContract Create(Employee employee,
                                          ContractType contractType,
                                          DateOnly startedAt,
                                          DateOnly? expiredAt = null)
        => new()
        {
            Employee = employee,
            ContractType = contractType,
            StartedAt = startedAt,
            ExpiredAt = expiredAt,
            TotalDuration = expiredAt.HasValue
                ? expiredAt.Value.DayNumber - startedAt.DayNumber
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static EmployeeContract Create(Guid employeeId,
                                          ContractType contractType,
                                          DateOnly startedAt,
                                          DateOnly? expiredAt = null)
        => new()
        {
            EmployeeId = employeeId,
            ContractType = contractType,
            StartedAt = startedAt,
            ExpiredAt = expiredAt,
            TotalDuration = expiredAt.HasValue 
                ? expiredAt.Value.DayNumber - startedAt.DayNumber
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(EmployeeContract entity,
                              Guid employeeId,
                              ContractType contractType,
                              DateOnly startedAt,
                              DateOnly? expiredAt = null)
    {
        entity.EmployeeId = employeeId;
        entity.ContractType = contractType;
        entity.StartedAt = startedAt;
        entity.ExpiredAt = expiredAt;
        entity.TotalDuration = expiredAt.HasValue
            ? expiredAt.Value.DayNumber - startedAt.DayNumber
            : null;
        entity.ModifiedAt = DateTime.UtcNow;
    }

    public static void Terminate(EmployeeContract entity,
                                 DateOnly expiredDate)
    {
        entity.ExpiredAt = expiredDate;
        entity.TotalDuration = expiredDate.DayNumber - entity.StartedAt.DayNumber;
    }

    #endregion
}
