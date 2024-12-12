using System.Net;

namespace HRLeaveManagement.Application.DTOs.Email;

public sealed record EmailResponse(bool IsSuccess,
                                   HttpStatusCode StatusCode,
                                   string? ErrorMessage = null);
