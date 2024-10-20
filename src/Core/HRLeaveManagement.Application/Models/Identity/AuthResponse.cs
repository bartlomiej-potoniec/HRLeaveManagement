﻿namespace HRLeaveManagement.Application.Models.Identity;

public record AuthResponse(string Id,
                           string UserName,
                           string Email,
                           string Token);