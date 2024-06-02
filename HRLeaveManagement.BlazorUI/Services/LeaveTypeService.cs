using AutoMapper;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveType;

namespace HRLeaveManagement.BlazorUI.Services;

public sealed class LeaveTypeService(IClient client, IMapper mapper) 
    : HttpServiceBase(client), ILeaveTypeService
{
    private readonly IMapper _mapper = mapper;
    
    public async Task<IEnumerable<LeaveTypeViewModel>> GetAll()
    {
        var leaveTypes = await _client.LeaveTypesAllAsync();
        var viewModels = _mapper.Map<IEnumerable<LeaveTypeViewModel>>(leaveTypes);

        return viewModels;
    }

    public async Task<LeaveTypeViewModel> GetDetails(int id)
    {
        var leaveType = await _client.LeaveTypesGETAsync(id);
        var viewModel = _mapper.Map<LeaveTypeViewModel>(leaveType);

        return viewModel;
    }

    public async Task<Response<Guid>> Create(LeaveTypeViewModel leaveType)
    {
        Response<Guid> response;

        try
        {
            var command = _mapper.Map<CreateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypesPOSTAsync(command);

            response = GenerateSuccessResponse("Leave Type created successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<Guid>(ex);
        }

        return response;
    } 

    public async Task<Response<Guid>> Update(int id, LeaveTypeViewModel leaveType)
    {
        Response<Guid> response;

        try
        {
            var command = _mapper.Map<UpdateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypesPATCHAsync(id, command);

            response = GenerateSuccessResponse("Leave Type updated successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<Guid>(ex);
        }

        return response;
    }

    public async Task<Response<Guid>> Delete(int id)
    {
        Response<Guid> response;

        try
        {
            await _client.LeaveTypesDELETEAsync(id);
            response = GenerateSuccessResponse("Leave Type removed successfully");
        }

        catch (ApiException ex)
        {
            response = base.ConvertApiExceptions<Guid>(ex);
        }

        return response;
    }

    private static Response<Guid> GenerateSuccessResponse(string message) 
        => new() 
        { 
            Message = message, 
            IsSuccess = true 
        };
}
