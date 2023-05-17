using System.Linq.Expressions;
using FSH.Framework.Core.Database;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Services;
using MongoDB.Driver;

namespace FSH.Framework.Persistence.Mongo;
public class MongoRepository<TDocument, TId> : IRepository<TDocument, TId> where TDocument : class, IBaseEntity<TId>
{
    private readonly IMongoDbContext _context;
    private readonly IMongoCollection<TDocument> _collection;
    private readonly IDateTimeService _dateTimeProvider;

    public MongoRepository(IMongoDbContext context, IDateTimeService dateTimeProvider)
    {
        _context = context;
        _collection = _context.GetCollection<TDocument>();
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> ExistsAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(predicate).AnyAsync(cancellationToken: cancellationToken)!;
    }

    public async Task<IReadOnlyList<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(predicate).ToListAsync(cancellationToken: cancellationToken)!;
    }

    public Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _collection.Find(predicate).SingleOrDefaultAsync(cancellationToken: cancellationToken)!;
    }

    public Task<TDocument?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return FindOneAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<TDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(document, new InsertOneOptions(), cancellationToken);
    }

    public async Task UpdateAsync(TDocument entity, CancellationToken cancellationToken = default)
    {
        entity.UpdateModifiedProperties(_dateTimeProvider.DateTimeUtcNow, string.Empty);
        _ = await _collection.ReplaceOneAsync(x => x.Id!.Equals(entity.Id), entity, cancellationToken: cancellationToken);
    }

    public Task DeleteRangeAsync(IReadOnlyList<TDocument> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TDocument entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(d => d.Id!.Equals(id), cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
