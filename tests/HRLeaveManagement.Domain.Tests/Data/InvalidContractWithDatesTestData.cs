using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.Tests.Helpers;
using System.Collections;

namespace HRLeaveManagement.Domain.Tests.Data;

public class InvalidContractWithDatesTestData : IEnumerable<object[]?>
{
    public IEnumerator<object[]?> GetEnumerator()
    {
        Employee employeeDouble = EmployeeHelper.CreateEmployee();

        List<EmployeeContract> employeeContractsWithindefiniteTerm = [
            EmployeeContract.Create(employeeDouble, ContractType.B2B, startedAt: new DateOnly(2025, 2, 1), expiredAt: null)
        ];

        List<EmployeeContract> employeeContractsWithConstantTerm = [
            EmployeeContract.Create(employeeDouble, ContractType.Employment, startedAt: new DateOnly(2023, 1, 1), expiredAt: new DateOnly(2024, 1, 1)),
            EmployeeContract.Create(employeeDouble, ContractType.B2B, startedAt: new DateOnly(2025, 1, 1), expiredAt: new DateOnly(2026, 1, 1)),
        ];

        yield return [
            new DateOnly(2023, 6, 12),
            new DateOnly(2025, 12, 12),
            employeeContractsWithindefiniteTerm,
            "Cannot define another contract during the indefinite-term contract"
        ];

        yield return [
            new DateOnly(2023, 6, 12),
            new DateOnly(2025, 2, 2),
            employeeContractsWithindefiniteTerm,
            "Cannot define another contract during the indefinite-term contract"
        ];

        yield return [
            new DateOnly(2022, 1, 1),
            new DateOnly(2023, 5, 5),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2023, 6, 6),
            new DateOnly(2025, 5, 5),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2025, 6, 6),
            new DateOnly(2027, 3, 3),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2023, 1, 1),
            new DateOnly(2024, 1, 1),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2025, 1, 1),
            new DateOnly(2026, 1, 1),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2023, 2, 2),
            new DateOnly(2023, 12, 12),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2025, 2, 2),
            new DateOnly(2025, 12, 12),
            employeeContractsWithConstantTerm,
            "Cannot define another contract during the current contract"
        ];

        yield return [
            new DateOnly(2024, 6, 6),
            null,
            employeeContractsWithConstantTerm,
            "Cannot define indefinite-term contract during the current contract"
        ];

        yield return [
            new DateOnly(2025, 12, 31),
            null,
            employeeContractsWithConstantTerm,
            "Cannot define indefinite-term contract during the current contract"
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
