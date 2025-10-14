namespace HRLeaveManagement.Application.Contracts.Identity;

public interface IEmailService
{
    Task SendRegistrationEmailAsync(string email,
                                    string firstName,
                                    string userName,
                                    string password,
                                    string confirmationLink,
                                    CancellationToken cancellationToken);
    Task SendEmployeeCreationEmailAsync(string email, string firstname, CancellationToken cancellationToken);
    Task SendDepartmentCreationEmailAsync(string email, int departmentId, string departmentName, CancellationToken cancellationToken);
    Task SendEmployeeContractCreationEmailAsync(string email, string contractType, string employeeName, CancellationToken cancellationToken);
    Task SendEmployeeUpdatingEmailAsync(string email, Guid employeeId, string firstName, string lastName, CancellationToken cancellationToken);
    Task SendLeaveAllocationUdpatingEmailAsync(string requestingUserEmail,
                                               Guid employeeId,
                                               string employeeFirstName,
                                               string employeeLastName,
                                               int? availableDays,
                                               CancellationToken cancellationToken);
    Task SendLeaveRequestCreationEmail(string requestingUserEmail,
                                       string requesterFullName,
                                       string approverFullName,
                                       string leaveTypeName,
                                       DateOnly leaveStartedAt,
                                       DateOnly leaveEndedAt,
                                       CancellationToken cancellationToken);
    Task SendLeaveRequestCancelationEmail(string requestingUserEmail,
                                          string requestingUserName,
                                          string leaveTypeName,
                                          DateTime leaveRequestCreatedAt,
                                          CancellationToken cancellationToken);
    Task SendLeaveRequestApprovalEmailAsync(string requestingUserEmail,
                                            string requestingUserName,
                                            string leaveTypeName,
                                            DateTime leaveRequestCreatedAt,
                                            CancellationToken cancellationToken);
    Task SendLeaveRequestRejectionEmailAsync(string requestingUserEmail,
                                             string requestingUserName,
                                             string leaveTypeName,
                                             DateTime leaveRequestCreatedAt,
                                             CancellationToken cancellationToken);
    string GenerateEmailConfirmationLink(string userId, string token, CancellationToken cancellationToken);
}
