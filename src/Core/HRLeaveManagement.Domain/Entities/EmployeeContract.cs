using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeContract
{
    public int Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public ContractType ContractType { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }

    private EmployeeContract() { }


    // Factory Methods
    public static EmployeeContract Create(Guid employeeId,
                                            ContractType contractType,
                                            DateTime startedAt,
                                            DateTime expiredAt)
        => new()
        {
            EmployeeId = employeeId,
            ContractType = contractType,
            StartedAt = startedAt,
            ExpiredAt = expiredAt,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };

    public static void Update(EmployeeContract entity,
                              Guid employeeId,
                              ContractType contractType,
                              DateTime startedAt,
                              DateTime expiredAt)
    {
        entity.EmployeeId = employeeId;
        entity.ContractType = contractType;
        entity.StartedAt = startedAt;
        entity.ExpiredAt = expiredAt;
    }
}
