﻿using HRLeaveManagement.Domain.Enums;

namespace HRLeaveManagement.Domain;

public class LeaveRequest
{
    public int Id { get; set; }

    public Guid RequestingEmployeeId { get; set; }

    public int LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public int TotalDays { get; set; }

    public Guid SubstitutorId { get; set; }
    public string? Comment { get; set; }

    public Guid ApproverId { get; set; }
    public string? ApproverComment { get; set; }

    public RequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DecidedAt { get; set; }
}
