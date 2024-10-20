using AutoMapper;
using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveAllocationService(IClient client,
                                           ILocalStorageService localStorage,
                                           IMapper mapper)
    : HttpServiceBase(client, localStorage), ILeaveAllocationService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response<Guid>> CreateLeaveAllocations(int leaveTypeId)
    {
        Response<Guid> response;

		try
		{
            var command = new CreateLeaveAllocationCommand { LeaveTypeId = leaveTypeId };
            await _client.LeaveAllocationsPOSTAsync(command);

            response = base.GenerateSuccessResponse("Leave allocations created successfully");
		}

		catch (ApiException ex)
		{
            response = base.ConvertApiExceptions<Guid>(ex);
		}

        return response;
    }
}
