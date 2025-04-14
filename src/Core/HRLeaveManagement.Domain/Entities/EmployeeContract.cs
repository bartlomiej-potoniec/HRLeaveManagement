using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain.Entities;

public class EmployeeContract
{
    public int Id { get; private set; }

    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

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
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        var employeeContracts = employee.EmploymentContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        return new()
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
    }

    public static EmployeeContract Create(Employee employee,
                                          ContractType contractType,
                                          DateTime startedAt,
                                          DateTime? expiredAt = null)
    {
        if (employee is null)
        {
            throw new ArgumentException("Employee must be included");
        }

        var employeeContracts = employee.EmploymentContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        return new()
        {
            Employee = employee,
            ContractType = contractType,
            StartedAt = DateOnly.FromDateTime(startedAt),
            ExpiredAt = expiredAt.HasValue
                ? DateOnly.FromDateTime(expiredAt.Value)
                : null,
            TotalDuration = expiredAt.HasValue
                ? (expiredAt - startedAt).Value.Days
                : null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    public static void Update(EmployeeContract employeeContract,
                              ContractType contractType,
                              DateOnly startedAt,
                              DateOnly? expiredAt = null)
    {
        if (employeeContract is null)
        {
            throw new ArgumentException("Employee contract must be included");
        }

        var employeeContracts = employeeContract.Employee.EmploymentContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        employeeContract.ContractType = contractType;
        employeeContract.StartedAt = startedAt;
        employeeContract.ExpiredAt = expiredAt;
        employeeContract.TotalDuration = expiredAt.HasValue
            ? expiredAt.Value.DayNumber - startedAt.DayNumber
            : null;
        employeeContract.ModifiedAt = DateTime.UtcNow;
    }

    public static void Update(EmployeeContract employeeContract,
                              ContractType contractType,
                              DateTime startedAt,
                              DateTime? expiredAt = null)
    {
        if (employeeContract is null)
        {
            throw new ArgumentException("Employee contract must be included");
        }

        var employee = employeeContract.Employee;
        var employeeContracts = employee.EmploymentContracts;
        ValidateBaseRules(employeeContracts, contractType, startedAt, expiredAt);

        employeeContract.ContractType = contractType;
        employeeContract.StartedAt = DateOnly.FromDateTime(startedAt);
        employeeContract.ExpiredAt = expiredAt.HasValue
            ? DateOnly.FromDateTime(expiredAt.Value)
            : null;
        employeeContract.TotalDuration = expiredAt.HasValue
            ? (expiredAt - startedAt).Value.Days
            : null;
        employeeContract.ModifiedAt = DateTime.UtcNow;
    }

    public static void Terminate(EmployeeContract employeeContract, DateOnly expiredDate)
    {
        if (expiredDate < employeeContract.StartedAt)
        {
            throw new InvalidOperationException("Contract termination date must be greater than start date");
        }

        employeeContract.ExpiredAt = expiredDate;
        employeeContract.TotalDuration = expiredDate.DayNumber - employeeContract.StartedAt.DayNumber;
    }

    public static void Terminate(EmployeeContract employeeContract, DateTime expiredDate)
    {
        if (DateOnly.FromDateTime(expiredDate) < employeeContract.StartedAt)
        {
            throw new InvalidOperationException("Contract termination date must be greater than start date");
        }

        employeeContract.ExpiredAt = DateOnly.FromDateTime(expiredDate);
        employeeContract.TotalDuration = employeeContract.ExpiredAt.Value.DayNumber - employeeContract.StartedAt.DayNumber;
    }

    private static void ValidateBaseRules(IEnumerable<EmployeeContract> employeeContracts,
                                          ContractType contractType,
                                          DateOnly startedAt,
                                          DateOnly? expiredAt = null)
    {
        if (expiredAt.HasValue && expiredAt.Value < startedAt)
        {
            throw new InvalidOperationException("Contract expiration date must be greater than start date");
        }

        foreach (var contract in employeeContracts)
        {
            if (!contract.ExpiredAt.HasValue && 
                (!expiredAt.HasValue || expiredAt.Value > contract.StartedAt))
            {
                throw new InvalidOperationException("Cannot define another contract during the indefinite-term contract");
            }

            if (contract.ExpiredAt.HasValue && 
                expiredAt.HasValue &&
                (expiredAt.Value > contract.StartedAt && startedAt < contract.ExpiredAt))
            {
                throw new InvalidOperationException("Cannot define another contract during the current contract");
            }

            if (contract.ExpiredAt.HasValue &&
                !expiredAt.HasValue && 
                startedAt < contract.ExpiredAt)
            {
                throw new InvalidOperationException("Cannot define indefinite-term contract during the current contract");
            }
        }
    }

    private static void ValidateBaseRules(IEnumerable<EmployeeContract> employeeContracts,
                                          ContractType contractType,
                                          DateTime startedAt,
                                          DateTime? expiredAt = null)
    {
        if (expiredAt.HasValue && expiredAt < startedAt)
        {
            throw new InvalidOperationException("Contract expiration date must be greater than start date");
        }

        foreach (var contract in employeeContracts)
        {
            if (!contract.ExpiredAt.HasValue &&
                (!expiredAt.HasValue || DateOnly.FromDateTime(expiredAt.Value) > contract.StartedAt))
            {
                throw new InvalidOperationException("Cannot define another contract during the indefinite-term contract");
            }

            if (contract.ExpiredAt.HasValue &&
                expiredAt.HasValue &&
                (DateOnly.FromDateTime(expiredAt.Value) > contract.StartedAt && 
                 DateOnly.FromDateTime(startedAt) < contract.ExpiredAt))
            {
                throw new InvalidOperationException("Cannot define another contract during the current contract");
            }

            if (contract.ExpiredAt.HasValue &&
                !expiredAt.HasValue &&
                DateOnly.FromDateTime(startedAt) < contract.ExpiredAt)
            {
                throw new InvalidOperationException("Cannot define indefinite-term contract during the current contract");
            }
        }
    }

    #endregion
}
