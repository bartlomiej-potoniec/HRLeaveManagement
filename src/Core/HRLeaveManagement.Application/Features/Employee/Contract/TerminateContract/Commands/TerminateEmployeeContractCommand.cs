namespace HRLeaveManagement.Application.Features.Employee.Contract.TerminateContract.Commands;

public sealed record TerminateEmployeeContractCommand(Guid EmployeeId, int ContractId);
