namespace HRLeaveManagement.Application.DTOs;

public record PagedResult<TData>
{
    public List<TData> Items { get; }
    public int? TotalPages { get; }
    public int? ItemsFrom { get; }
    public int? ItemsTo { get; }
    public int TotalItemsCount { get; }

    public PagedResult(List<TData> items, int totalCount, int? pageSize, int? pageNumber)
    {
        Items = items;
        TotalItemsCount = totalCount;

        ItemsFrom = pageSize is null || pageNumber is null 
            ? null
            : pageSize * (pageNumber - 1) + 1;

        ItemsTo = pageSize is null 
            ? null 
            : ItemsFrom + pageSize - 1;

        TotalPages = pageSize is null
            ? null
            : (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
