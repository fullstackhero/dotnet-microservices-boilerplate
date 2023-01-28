namespace FSH.Infrastructure.Pagination;

public class PagedList<T>
{
    public IList<T> Data { get; }
    public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        Data = items as IList<T> ?? new List<T>(items);
    }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalItems { get; }
    public bool IsFirstPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == TotalPages;
}
