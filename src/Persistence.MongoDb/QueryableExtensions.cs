using AutoMapper;
using AutoMapper.QueryableExtensions;
using FSH.Infrastructure.Pagination;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FSH.Persistence.MongoDb;

public static class QueryableExtensions
{
    public static async Task<PagedList<R>> ApplyPagingAsync<T, R>(this IMongoQueryable<T> collection, IConfigurationProvider configuration, int page = 1, int resultsPerPage = 10, CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;

        if (resultsPerPage <= 0) resultsPerPage = 10;
        var skipSize = (page - 1) * resultsPerPage;
        var isEmpty = !await collection.AnyAsync(cancellationToken: cancellationToken);
        if (isEmpty) return new(Enumerable.Empty<R>(), 0, 0, 0);

        var totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
        var data = collection.Skip(skipSize).Take(resultsPerPage).ProjectTo<R>(configuration).ToList();

        return new PagedList<R>(data, totalItems, page, resultsPerPage);
    }
}
