using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using Blazored.LocalStorage;
using AutoMapper;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveAllocationService(IClient client,
                                           ILocalStorageService localStorage,
                                           IMapper mapper)
    : HttpServiceBase(client, localStorage), ILeaveAllocationService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Response> CreateLeaveAllocations(int leaveTypeId)
    {
        Response response;

		try
		{
            var command = new CreateLeaveAllocationsCommand {  };
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
