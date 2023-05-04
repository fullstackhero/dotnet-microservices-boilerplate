namespace FSH.Framework.Core.Pagination;
public class PagedList<T>
{
    public IList<T> Data { get; }
    public PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        if (totalItems > 0)
        {
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
        Data = items as IList<T> ?? new List<T>(items);
    }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalItems { get; }
    public bool IsFirstPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == TotalPages && TotalPages > 0;
}