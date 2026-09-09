using HRLeaveManagement.Application.Contracts.Application;
using HRLeaveManagement.Application.Contracts.Persistence.ContextFactories;
using HRLeaveManagement.Application.DTOs.Employees;
using HRLeaveManagement.Domain.Employee;
using HRLeaveManagement.Domain.Employee.Contract;
using HRLeaveManagement.Domain.Document;
using HRLeaveManagement.Domain.Employee.Education;
using HRLeaveManagement.Domain.Employee.Experience;

namespace HRLeaveManagement.Application.Subservices;

public sealed class EmployeeSubservice(IEmployeeContextFactory employeeContextFactory,
                                       IEmployeeDocumentNumberUniqueChecker employeeDocumentRuleSet)
    : IEmployeeSubservice
{
    private readonly IEmployeeContextFactory _employeeContextFactory = employeeContextFactory;
    private readonly IEmployeeDocumentNumberUniqueChecker _employeeDocumentRuleSet = employeeDocumentRuleSet;

    public async Task<EmployeeContract> CreateEmployeeContract(Employee employee,
                                                               EmployeeContractRequest contractRequest,
                                                               CancellationToken cancellationToken = default)
    {
        var employeeWithContracts = _employeeContextFactory.AsEmployeeWithContracts(employee);

        var contractDocumentPayloads = (contractRequest.EmployeeDocuments ?? [])
            .Select(doc => new EmployeeDocumentPayload(
                doc.Title,
                doc.DocumentNumber,
                doc.FileUrl,
                doc.Description
            ));

        var contractDocuments = await EmployeeDocument.CreateManyAsync(
            _employeeDocumentRuleSet,
            contractDocumentPayloads,
            cancellationToken
        );

        var contract = employeeWithContracts.AddContract(
            contractRequest.ContractType,
            contractRequest.ContractDetails,
            DateOnly.FromDateTime(contractRequest.EmployeedFrom),
            contractRequest.EmployeedTo.HasValue 
                ? DateOnly.FromDateTime(contractRequest.EmployeedTo.Value)
                : null,
            contractDocuments
        );

        return contract;
    }

    public async Task<IReadOnlyList<EmployeeEducation>> CreateEmployeeEducations(Employee employee,
                                                                                 IEnumerable<EmployeeEducationRequest> educationRequests,
                                                                                 CancellationToken cancellationToken)
    {
        var employeeWithEducations = _employeeContextFactory.AsEmployeeWithEducations(employee);

        var educationPayloads = educationRequests
            .Select(edu => new EmployeeEducationPayload(
                edu.EducationType,
                edu.InstitutionName,
                DateOnly.FromDateTime(edu.EnrolledAt),
                edu.GraduatedAt.HasValue ? DateOnly.FromDateTime(edu.GraduatedAt.Value) : null,
                edu.EducationDetails,
                edu.EmployeeDocuments
                    ?.Select(doc => new EmployeeDocumentPayload(
                        doc.Title,
                        doc.DocumentNumber,
                        doc.FileUrl,
                        doc.Description
                    ))
        ));

        var educations = await employeeWithEducations.AddManyEducationsAsync(_employeeDocumentRuleSet, educationPayloads, cancellationToken);
        return educations;
    }

    public async Task<IReadOnlyList<EmployeeExperience>> CreateEmployeeExperiences(Employee employee,
                                                                                   IEnumerable<EmployeeExperienceRequest> experienceRequests,
                                                                                   CancellationToken cancellationToken)
    {
        var employeeWithExperiences = _employeeContextFactory.AsEmployeeWithExperiences(employee);

        var experiencePayloads = experienceRequests
            .Select(exp => new EmployeeExperiencePayload(
                exp.ContractType,
                exp.PreviousCompanyName,
                exp.Position,
                DateOnly.FromDateTime(exp.EmployedFrom),
                DateOnly.FromDateTime(exp.EmployedTo),
                exp.ExperienceDetails,
                exp.EmployeeDocuments
                    ?.Select(doc => new EmployeeDocumentPayload(
                        doc.Title,
                        doc.DocumentNumber,
                        doc.FileUrl,
                        doc.Description
                    ))
            ));

        var experiences = await employeeWithExperiences.AddManyExperiencesAsync(_employeeDocumentRuleSet, experiencePayloads, cancellationToken);
        return experiences;
    }

    public async Task UpdateEmployeeContracts(Employee employeeWithDetails,
                                              List<EmployeeContractDetailsRequest> contractDetailsRequests,
                                              CancellationToken cancellationToken = default)
    {
        var employeeWithContracts = _employeeContextFactory.AsEmployeeWithContracts(employeeWithDetails);

        // Delete contracts that are no longer in DTO
        foreach (var contract in employeeWithContracts.EmployeeContracts)
        {
            if (!contractDetailsRequests.Any(c => c.Id == contract.Id))
            {
                employeeWithContracts.RemoveContract(contract);
            }
        }

        // Add new contracts
        foreach (var dto in contractDetailsRequests.Where(c => c.Id is null))
        {
            var payloads = (dto.EmployeeDocuments ?? [])
                .Select(doc => new EmployeeDocumentPayload(
                    doc.Title,
                    doc.DocumentNumber,
                    doc.FileUrl,
                    doc.Description
                ));

            var documents = await EmployeeDocument.CreateManyAsync(_employeeDocumentRuleSet, payloads, cancellationToken);

            employeeWithContracts.AddContract(
                dto.ContractType,
                dto.ContractDetails,
                DateOnly.FromDateTime(dto.StartedAt),
                dto.ExpiredAt.HasValue ? DateOnly.FromDateTime(dto.ExpiredAt.Value) : null,
                documents
            );
        }

        // Update existing contracts
        foreach (var dto in contractDetailsRequests.Where(c => c.Id is not null))
        {
            var existingContract = employeeWithContracts.EmployeeContracts.FirstOrDefault(c => c.Id == dto.Id);

            if (existingContract is null)
            {
                continue;
            }

            // Delete contract documents that are no longer in DTO
            foreach (var document in existingContract.EmployeeDocuments)
            {
                if (dto.EmployeeDocuments.Any(doc => doc.Id == document.Id))
                {
                    existingContract.RemoveDocument(document);
                }
            }

            // Add new contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is null))
            {
                var document = await EmployeeDocument.CreateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );

                existingContract.AddDocument(document);
            }

            // Update existing contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is not null))
            {
                var existingDocument = existingContract.EmployeeDocuments.FirstOrDefault(doc => doc.Id == docuemntDto.Id);

                if (existingDocument is null)
                {
                    continue;
                }

                await existingDocument.UpdateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );
            }

            existingContract?.Update(
                employeeWithContracts,
                dto.ContractType,
                dto.ContractDetails,
                DateOnly.FromDateTime(dto.StartedAt),
                dto.ExpiredAt.HasValue ? DateOnly.FromDateTime(dto.ExpiredAt.Value) : null
            );
        }
    }

    public async Task UpdateEmployeeEducations(Employee employeeWithDetails,
                                               List<EmployeeEducationDetailsRequest> educationDetailsRequests,
                                               CancellationToken cancellationToken = default)
    {
        var employeeWithEducations = _employeeContextFactory.AsEmployeeWithEducations(employeeWithDetails);

        // Delete educations that are no longer in DTO
        foreach (var education in employeeWithEducations.EmployeeEducations)
        {
            if (!educationDetailsRequests.Any(c => c.Id == education.Id))
            {
                employeeWithEducations.RemoveEducation(education);
            }
        }

        // Add new educations
        foreach (var dto in educationDetailsRequests.Where(c => c.Id is null))
        {
            var payloads = (dto.EmployeeDocuments ?? [])
                .Select(doc => new EmployeeDocumentPayload(
                    doc.Title,
                    doc.DocumentNumber,
                    doc.FileUrl,
                    doc.Description
                ));

            var documents = await EmployeeDocument.CreateManyAsync(_employeeDocumentRuleSet, payloads, cancellationToken);

            employeeWithEducations.AddEducation(
                dto.EducationType,
                dto.InstitutionName,
                DateOnly.FromDateTime(dto.EnrolledAt),
                dto.GraduatedAt.HasValue ? DateOnly.FromDateTime(dto.GraduatedAt.Value) : null,
                dto.EducationDetails,
                documents
            );
        }

        // Update existing educations
        foreach (var dto in educationDetailsRequests.Where(c => c.Id is not null))
        {
            var existingEducation = employeeWithEducations.EmployeeEducations.FirstOrDefault(c => c.Id == dto.Id);

            if (existingEducation is null)
            {
                return;
            }

            // Delete contract documents that are no longer in DTO
            foreach (var document in existingEducation.EmployeeDocuments)
            {
                if (dto.EmployeeDocuments.Any(doc => doc.Id == document.Id))
                {
                    existingEducation.RemoveDocument(document);
                }
            }

            // Add new contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is null))
            {
                var document = await EmployeeDocument.CreateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );

                existingEducation.AddDocument(document);
            }

            // Update existing contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is not null))
            {
                var existingDocument = existingEducation.EmployeeDocuments.FirstOrDefault(doc => doc.Id == docuemntDto.Id);

                if (existingDocument is null)
                {
                    continue;
                }

                await existingDocument.UpdateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );
            }

            existingEducation?.Update(
                dto.EducationType,
                dto.InstitutionName,
                DateOnly.FromDateTime(dto.EnrolledAt),
                dto.GraduatedAt.HasValue ? DateOnly.FromDateTime(dto.GraduatedAt.Value) : null,
                dto.EducationDetails
            );
        }
    }

    public async Task UpdateEmployeeExperiences(Employee employeeWithDetails,
                                                List<EmployeeExperienceDetailsRequest> experienceDetailsRequests,
                                                CancellationToken cancellationToken = default)
    {
        var employeeWithExperiences = _employeeContextFactory.AsEmployeeWithExperiences(employeeWithDetails);

        // Delete experiences that are no longer in DTO
        foreach (var experience in employeeWithExperiences.EmployeeExperiences)
        {
            if (!experienceDetailsRequests.Any(c => c.Id == experience.Id))
            {
                employeeWithExperiences.RemoveExperience(experience);
            }
        }

        // Add new experiences
        foreach (var dto in experienceDetailsRequests.Where(c => c.Id is null))
        {
            var payloads = (dto.EmployeeDocuments ?? [])
                .Select(doc => new EmployeeDocumentPayload(
                    doc.Title,
                    doc.DocumentNumber,
                    doc.FileUrl,
                    doc.Description
                ));

            var documents = await EmployeeDocument.CreateManyAsync(_employeeDocumentRuleSet, payloads, cancellationToken);

            employeeWithExperiences.AddExperience(
                dto.ContractType,
                dto.PreviousCompanyName,
                dto.Position,
                DateOnly.FromDateTime(dto.EmployedFrom),
                DateOnly.FromDateTime(dto.EmployedTo),
                dto.ExperienceDetails,
                documents
            );
        }

        // Update existing experiences
        foreach (var dto in experienceDetailsRequests.Where(c => c.Id is not null))
        {
            var existingExperience = employeeWithExperiences.EmployeeExperiences.FirstOrDefault(c => c.Id == dto.Id);

            if (existingExperience is null)
            {
                continue;
            }

            // Delete contract documents that are no longer in DTO
            foreach (var document in existingExperience.EmployeeDocuments)
            {
                if (dto.EmployeeDocuments.Any(doc => doc.Id == document.Id))
                {
                    existingExperience.RemoveDocument(document);
                }
            }

            // Add new contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is null))
            {
                var document = await EmployeeDocument.CreateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );

                existingExperience.AddDocument(document);
            }

            // Update existing contract documents
            foreach (var docuemntDto in dto.EmployeeDocuments.Where(doc => doc.Id is not null))
            {
                var existingDocument = existingExperience.EmployeeDocuments.FirstOrDefault(doc => doc.Id == docuemntDto.Id);

                if (existingDocument is null)
                {
                    continue;
                }

                await existingDocument.UpdateAsync(
                    _employeeDocumentRuleSet,
                    docuemntDto.Title,
                    docuemntDto.DocumentNumber,
                    docuemntDto.FileUrl,
                    docuemntDto.Description,
                    cancellationToken
                );
            }

            existingExperience?.Update(
                dto.ContractType,
                dto.PreviousCompanyName,
                dto.Position,
                DateOnly.FromDateTime(dto.EmployedFrom),
                DateOnly.FromDateTime(dto.EmployedTo),
                dto.ExperienceDetails
            );
        }
    }
}
