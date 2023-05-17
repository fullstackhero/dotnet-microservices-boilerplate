using FSH.Framework.Core.Pagination;
using Mapster;
using MongoDB.Driver.Linq;

namespace FSH.Framework.Persistence.Mongo;
public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ApplyPagingAsync<T>(this IMongoQueryable<T> collection, int page = 1, int resultsPerPage = 10, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (resultsPerPage <= 0) resultsPerPage = 10;
        int skipSize = (page - 1) * resultsPerPage;
        bool isEmpty = !await collection.AnyAsync(cancellationToken: cancellationToken);
        if (isEmpty) return new(Enumerable.Empty<T>(), 0, 0, 0);
        int totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
        var data = collection.Skip(skipSize).Take(resultsPerPage).ToList();
        return new PagedList<T>(data, totalItems, page, resultsPerPage);
    }
    public static async Task<PagedList<TDto>> ApplyPagingAsync<T, TDto>(this IMongoQueryable<T> collection, int page = 1, int resultsPerPage = 10, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (resultsPerPage <= 0) resultsPerPage = 10;
        int skipSize = (page - 1) * resultsPerPage;
        bool isEmpty = !await collection.AnyAsync(cancellationToken: cancellationToken);
        if (isEmpty) return new(Enumerable.Empty<TDto>(), 0, 0, 0);
        int totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
        var data = collection.Skip(skipSize).Take(resultsPerPage).ProjectToType<TDto>();
        return new PagedList<TDto>(data, totalItems, page, resultsPerPage);
    }
}
