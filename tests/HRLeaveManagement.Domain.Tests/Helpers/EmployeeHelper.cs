using HRLeaveManagement.Domain.Enums;
using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class EmployeeHelper
{
    internal static Employee CreateEmployee(string position = "Engineer",
                                            string responsibilities = "Engineering",
                                            string residentialAddress1 = "Cracow 12/4",
                                            string registeredAddress = "Warsaw 15/8",
                                            string? residentialAddress2 = null,
                                            string? remoteWorkAddress = null,
                                            Section? section = null,
                                            Employee? leader = null)
        => Employee.Create(
            position,
            responsibilities,
            residentialAddress1,
            registeredAddress,
            residentialAddress2,
            remoteWorkAddress,
            section,
            leader
        );
     
    internal static async Task<Employee> CreateEmployeeWithWorkRequestListAsync(params (DateOnly Start, DateOnly End)[] workRequestDateRanges)
    {
        Employee requestingEmployee = CreateEmployee();
        Employee approver = CreateEmployee();

        Mock<IWorkRequestRuleSet> workRequestRuleSetMock = WorkRequestTestHelper.CreateWorkRequestRuleSetMock();
        WorkRequestTestHelper.SetupIsRequestApproverSuperiorOfEmployeeAsyncToReturnResult(workRequestRuleSetMock, isRuleFailed: false);

        foreach (var (startedAt, endedAt) in workRequestDateRanges)
        {
            WorkRequestTestHelper workRequest = new();
            await workRequest.InitializeBase(requestingEmployee, startedAt, endedAt, approver, workRequestRuleSetMock);

            requestingEmployee.AddWorkRequest(workRequest);
        }    

        return requestingEmployee;
    }

    internal static Employee CreateEmployeeWithContractList(params (DateOnly Start, DateOnly? End)[] contractDateRanges)
    {
        Employee employee = CreateEmployee();

        ContractType contractType = ContractType.Employment;
        string contractDetails = "Details for contract";

        Mock<IEmployeeDocumentRuleSet> employeeDocumentRuleSetMock = EmployeeDocumentHelper.CreateEmployeeDocumentRuleSetMock();
        EmployeeDocumentHelper.SetupIsDocumentNumberUniqueAsyncToReturnValue(employeeDocumentRuleSetMock, isRuleFailed: false);

        foreach (var (startedAt, expiredAt) in contractDateRanges)
        {
            employee.AddContract(contractType, contractDetails, startedAt, expiredAt);
        }

        return employee;
    }

    internal static Employee CreateEmployeeWithExperienceList(params (DateOnly Start, DateOnly End)[] experienceDateRanges)
    {
        Employee employee = CreateEmployee();

        ContractType contractType = ContractType.Employment;
        string previousCompanyName = "Company sp. z.o.o.";
        string position = "Engineer";

        foreach (var (employedFrom, employedTo) in experienceDateRanges)
        {
            employee.AddExperience(contractType, previousCompanyName, position, employedFrom, employedTo);
        }

        return employee;
    }

    internal static Employee CreateEmployeeWithEducationList(params (DateOnly Start, DateOnly? End)[] educationDateRanges)
    {
        Employee employee = CreateEmployee();

        EducationType educationType = EducationType.PostSecondary;
        string institutionName = "High School of Engineering";
        string educationDetails = "Details of education";

        foreach (var (enrolledAt, graduatedAt) in educationDateRanges)
        {
            employee.AddEducation(educationType, institutionName, enrolledAt, graduatedAt, educationDetails);
        }

        return employee;
    }

    internal static Employee CreateEmployeeWithRemoteWorkList(params (int Year, int AvailableDays)[] remoteWorkLimitParams)
    {
        Employee employee = CreateEmployee();

        foreach (var (year, availableDays) in remoteWorkLimitParams)
        {
            var remoteWorkLimit = RemoteWorkLimit.Create(employee, year, year, availableDays);
            employee.AddRemoteWorkLimit(remoteWorkLimit);
        }

        return employee;
    }

    internal static async Task<Employee> CreateEmployeeWithLeaveRequestListAsync(Employee? approver = null,
                                                                                 params (DateOnly Start, DateOnly End)[] leaveRequestDateRanges)
    {
        Employee requestingEmployee = CreateEmployee();
        Employee substitutor = CreateEmployee();
        approver ??= CreateEmployee();

        Domain.Entities.LeaveType leaveType = await LeaveTypeHelper.CreateLeaveTypeAsync("Vacation leave");

        Mock<ILeaveRequestRuleSet> leaveRequestRuleSetMock = new();
        leaveRequestRuleSetMock
            .Setup(rule => rule.IsRequestApproverSuperiorOfEmployeeAsync(requestingEmployee, substitutor, CancellationToken.None))
            .ReturnsAsync(true);

        foreach (var (startedAt, endedAt) in leaveRequestDateRanges)
        {
            LeaveRequest leaveRequest = await LeaveRequest.CreateAsync(
                leaveRequestRuleSetMock.Object,
                requestingEmployee,
                leaveType,
                startedAt,
                endedAt,
                approver,
                substitutor
            );

            requestingEmployee.AddLeaveRequest(leaveRequest);
        }

        return requestingEmployee;
    }

    internal static Employee CreateEmployeeWithTimeRegisterList(
        params (DateOnly RegisterDate, TimeOnly Start, TimeOnly End, TimeOnly? BreakStart, TimeOnly? BreakEnd)[] timeRegisterParams
    ) 
    {
        Employee employee = CreateEmployee();

        foreach (var (registerDate, start, end, breakStart, breakEnd) in timeRegisterParams)
        {
            var timeRegister = TimeRegister.Create(employee, registerDate, start, end, breakStart, breakEnd);
            employee.AddTimeRegister(timeRegister);
        }

        return employee;
    }
}
