using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace HRLeaveManagement.BlazorUI.Layout;

public partial class FormTextField
{
    [Parameter] 
    public int Xs { get; set; } = 12;
    
    [Parameter] 
    public string Label { get; set; }
    
    [Parameter] 
    public string ForId { get; set; }
    
    [Parameter] 
    public string Value { get; set; }

    [Parameter] 
    public Expression<Func<string>> For { get; set; }
}
