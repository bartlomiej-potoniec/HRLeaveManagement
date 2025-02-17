namespace HRLeaveManagement.BlazorUI.ViewModels;

public class PagedViewModel<TViewModel>
{
    public List<TViewModel> Items { get; set; }
    public int? TotalPages { get; set; }
    public int? ItemsFrom { get; set; }
    public int? ItemsTo { get; set; }
    public int TotalItemsCount { get; set; }
}
