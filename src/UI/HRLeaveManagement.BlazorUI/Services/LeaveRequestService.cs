using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveRequests;
using Blazored.LocalStorage;
using AutoMapper;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveAllocations;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveRequestService(IClient client,
                                        ILocalStorageService localStorage,
										IMapper mapper)
    : HttpServiceBase(client, localStorage), ILeaveRequestService
{
	private readonly IMapper _mapper = mapper;

    public async Task<LeaveRequestViewModel> GetByIdAsync(int id)
    {
        var leaveRequestDetails = await _client.LeaveRequestsGETAsync(id);
        var viewModel = _mapper.Map<LeaveRequestViewModel>(leaveRequestDetails);

        return viewModel;
    }

    public async Task<AdminLeaveRequestViewModel> GetForAdminAsync()
    {
        var leaveRequests = await _client.LeaveRequestsAllAsync(isUserLoggedIn: false);

        var viewModel = new AdminLeaveRequestViewModel
        {
            TotalRequests = leaveRequests.Count,
            ApprovedRequests = leaveRequests.Count(lr => lr.IsApproved is true),
            RejectedRequests = leaveRequests.Count(lr => lr.IsApproved is false),
            PendingRequests = leaveRequests.Count(lr => lr.IsApproved is null),
            LeaveRequests = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests)
        };

        return viewModel;
    }

    public async Task<EmployeeLeaveRequestViewModel> GetForEmployeeAsync()
    {
        var leaveRequests = await _client.LeaveRequestsAllAsync(isUserLoggedIn: true);
        var leaveAllocations = await _client.LeaveAllocationsAllAsync();

        var viewModel = new EmployeeLeaveRequestViewModel
        {
            LeaveAllocations = _mapper.Map<List<LeaveAllocationViewModel>>(leaveAllocations),
            LeaveRequests = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests)
        };

        return viewModel;
    }

    public async Task<Response<Guid>> CreateAsync(LeaveRequestViewModel leaveRequest)
    {
        Response<Guid> response;

		try
		{
			var command = _mapper.Map<CreateLeaveRequestCommand>(leaveRequest);
			await _client.LeaveRequestsPOSTAsync(command);

			response = base.GenerateSuccessResponse("Leave Request created successfully");
		}

		catch (ApiException ex)
		{
			response = base.ConvertApiExceptions<Guid>(ex);
		}

		return response;
    }

    public async Task<Response<Guid>> Cancel(int id)
    {
        Response<Guid> response;

        try
        {
            var request = new CancelLeaveRequestCommand { Id = id };
            await _client.CancelAsync(request);

            response = base.GenerateSuccessResponse("Leave Request Cancelled successfully");
        }

        catch (ApiException ex)
        {
            response = ConvertApiExceptions<Guid>(ex);
        }

        return response;
    }

    public async Task<Response<Guid>> ApproveAsync(int id, bool approvalStatus)
    {
        Response<Guid> response;

        try
        {
            var command = new ChangeLeaveRequestApprovalCommand
            {
                Id = id,
                IsApproved = approvalStatus
            };

            await _client.ApproveAsync(command);

            response = base.GenerateSuccessResponse("Approval status changed successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<Guid>(ex);
        }

        return response;
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
