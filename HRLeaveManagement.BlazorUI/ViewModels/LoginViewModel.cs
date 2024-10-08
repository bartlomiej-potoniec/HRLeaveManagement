﻿using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
