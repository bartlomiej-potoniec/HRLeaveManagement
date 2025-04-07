using HRLeaveManagement.BlazorUI.ViewModels.Users;

namespace HRLeaveManagement.BlazorUI.Layout.Table;

public partial class HrUserAccountStatusFilter<T> : HrFilter<T> where T : class
{
    protected override void LoadItems() 
        => Items = Data
            .OfType<UserDetailsViewModel>()
            .GroupBy(s => s.AccountStatus)
            .Select(g => g.Key)
            .ToList();
}
