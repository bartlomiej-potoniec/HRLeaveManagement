﻿using HRLeaveManagement.Application.Models.Identity;

namespace HRLeaveManagement.Application.DTOs;

public sealed record LeaveRequestDTO
{
    public required int Id { get; init; }
    public required Employee Employee { get; init; }
    public required string RequestingEmployeeId { get; init; }
    public required LeaveTypeDTO LeaveType { get; init; }
    public required DateTime RequestedAt { get; init; }
    public required DateTime StartedAt { get; init; }
    public required DateTime EndedAt { get; init; }
    public bool? IsApproved { get; init; }
}
