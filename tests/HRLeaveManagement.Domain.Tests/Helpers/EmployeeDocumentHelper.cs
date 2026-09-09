using HRLeaveManagement.Domain.Document;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class EmployeeDocumentHelper
{
    internal static async Task<EmployeeDocument> CreateEmployeeDocumentAsync(IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet,
                                                                             string title = "Document title",
                                                                             string documentNumber = "No.1",
                                                                             string? description = "Document description",
                                                                             string fileUrl = "/sth/sth1/sth2/")
        => await EmployeeDocument.CreateAsync(
            employeeDocumentRuleSet,
            title,
            documentNumber,
            fileUrl,
            description
        );

    internal static Mock<IEmployeeDocumentNumberUniqueChecker> CreateEmployeeDocumentRuleSetMock() => new();

    internal static void SetupIsDocumentNumberUniqueAsyncToReturnValue(Mock<IEmployeeDocumentNumberUniqueChecker> employeeDocumentRuleSetMock,
                                                                       bool isRuleFailed)
        => employeeDocumentRuleSetMock
            .Setup(rule => rule.IsEligibleAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(!isRuleFailed);
}
