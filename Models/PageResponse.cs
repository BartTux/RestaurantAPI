namespace RestaurantAPI.Models;

public class PageResponse<TResponse>
{
    public IEnumerable<TResponse> Items { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    public int TotalItemsCount { get; set; }

    public PageResponse(IEnumerable<TResponse> items,
                        int totalItemsCount,
                        int pageSize,
                        int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
    }
}
