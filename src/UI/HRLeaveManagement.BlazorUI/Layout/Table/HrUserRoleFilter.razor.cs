using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Layout.Table;

public partial class HrUserRoleFilter<T> : HrFilter<T> where T : class
{
    protected override void LoadItems()
        => Items = Data
            .OfType<UserDetailsViewModel>()
            .GroupBy(r => r.Roles)
            .SelectMany(u => u.Key)
            .Distinct()
            .ToList();
}
