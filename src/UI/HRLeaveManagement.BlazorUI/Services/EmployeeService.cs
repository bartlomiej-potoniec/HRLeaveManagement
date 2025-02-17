using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public class EmployeeService(IClient client,
                             ILocalStorageService localStorage,
                             IMapper mapper)
	: HttpServiceBase(client, localStorage), IEmployeeService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response<List<EmployeeViewModel>>> GetAllAsync()
    {
        Response<List<EmployeeViewModel>> response;

		try
		{
            var employees = await _client.EmployeesAllAsync();
			var viewModel = _mapper.Map<List<EmployeeViewModel>>(employees);
			 
			response = base.GenerateSuccessResponse("Employees GET operation successful", viewModel);
		}

		catch (ApiException ex)
		{
			response = base.ConvertApiExceptions<List<EmployeeViewModel>>(ex);
		}
		  
		return response;
    }

	public async Task<Response<EmployeeDetailsViewModel>> GetWithDetailsByIdAsync(Guid id)
	{
		Response<EmployeeDetailsViewModel> response;

		try
		{
			var employee = await _client.EmployeesGETAsync(id);
			var viewModel = _mapper.Map<EmployeeDetailsViewModel>(employee);

			response = base.GenerateSuccessResponse("Employee GET operation successful", viewModel);
		}

		catch (ApiException ex)
		{
			response = base.ConvertApiExceptions<EmployeeDetailsViewModel>(ex);
		}

		return response;
	}

	public async Task<Response<EmployeeViewModel>> CreateAsync(CreateEmployeeDetailsViewModel viewModel)
	{
		Response<EmployeeViewModel> response;

        try
        {
			var command = _mapper.Map<CreateEmployeeWithDetailsCommand>(viewModel);
			var employeeDetails = await _client.EmployeesPOSTAsync(command);
			var result = _mapper.Map<EmployeeViewModel>(employeeDetails);

			response = base.GenerateSuccessResponse<EmployeeViewModel>("Employee created successfully", result);
		}

		catch (ApiException ex)
		{
			response = base.ConvertApiExceptions<EmployeeViewModel>(ex);
		}

		return response;
	}

	public async Task<Response> UpdateWithDetailsAsync(EditEmployeeDetailsViewModel viewModel)
	{
		Response response;

		try
		{
			var command = _mapper.Map<UpdateEmployeeWithDetailsCommand>(viewModel);
			await _client.EmployeesPUTAsync(command.Id, command);

			response = base.GenerateSuccessResponse("Employee updated successfuly");
		}

		catch (ApiException ex)
		{
			response = base.ConvertApiExceptions(ex);
		}

		return response;
	}
}
