namespace HRLeaveManagement.Application.Features.Employee.Commands;

public sealed record TerminateEmployeeContractCommand(Guid EmployeeId, int ContractId);
