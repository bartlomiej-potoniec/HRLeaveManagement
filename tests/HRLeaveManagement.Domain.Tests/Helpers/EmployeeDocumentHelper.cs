using HRLeaveManagement.Domain.RuleContracts;

namespace HRLeaveManagement.Domain.Tests.Helpers;

internal class EmployeeDocumentHelper
{
    internal static async Task<EmployeeDocument> CreateEmployeeDocumentAsync(IEmployeeDocumentRuleSet employeeDocumentRuleSet,
                                                                             string title = "Document title",
                                                                             string documentNumber = "No.1",
                                                                             string? description = "Document description",
                                                                             string fileUrl = "/sth/sth1/sth2/")
        => await EmployeeDocument.CreateSingleAsync(
            employeeDocumentRuleSet,
            title,
            documentNumber,
            fileUrl,
            description
        );

    internal static Mock<IEmployeeDocumentRuleSet> CreateEmployeeDocumentRuleSetMock() => new();

    internal static void SetupIsDocumentNumberUniqueAsyncToReturnValue(Mock<IEmployeeDocumentRuleSet> employeeDocumentRuleSetMock,
                                                                       bool isRuleFailed)
        => employeeDocumentRuleSetMock
            .Setup(rule => rule.IsDocumentNumberUniqueAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(!isRuleFailed);
}
