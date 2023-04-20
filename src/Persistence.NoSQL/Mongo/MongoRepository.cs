using FSH.Microservices.Core.Database;
using FSH.Microservices.Core.Domain;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace FSH.Microservices.Persistence.NoSQL.Mongo;
public class MongoRepository<TDocument, TId> : IRepository<TDocument, TId> where TDocument : class, IBaseEntity<TId>
{
    private readonly IMongoDbContext _context;
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IMongoDbContext context)
    {
        _context = context;
        _collection = _context.GetCollection<TDocument>();
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

    public Task<TDocument> UpdateAsync(TDocument entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

    public Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
