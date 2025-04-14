namespace HRLeaveManagement.Domain.Tests.Helpers;

public class EmployeeHelper
{
    public static Employee CreateEmployee(string position = "Engineer",
                                          string responsibilities = "Engineering",
                                          int? sectionId = 1,
                                          Guid? leaderId = default,
                                          IEnumerable<EmployeeEducation>? educations = null,
                                          IEnumerable<EmployeeContract>? contracts = null,
                                          IEnumerable<EmployeeExperience>? experiences = null)
        => Employee.Create(position, responsibilities, sectionId, leaderId, educations, contracts, experiences);
}
