using MongoDB.Driver;

namespace FSH.Persistence.MongoDb;

internal class MongoRepository<TEntity, TId> : IMongoRepository<TEntity, TId>
    where TEntity : class
{
    private readonly IMongoDbContext _context;
    protected readonly IMongoCollection<TEntity> DbSet;

    public MongoRepository(IMongoDbContext context, IMongoCollection<TEntity> dbSet)
    {
        _context = context;
        DbSet = dbSet;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
        return entity;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
