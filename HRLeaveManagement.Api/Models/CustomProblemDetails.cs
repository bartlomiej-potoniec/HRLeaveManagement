﻿using Microsoft.AspNetCore.Mvc;

namespace HRLeaveManagement.Api.Models;

public class CustomProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = (Dictionary<string, string[]>)[];
}
