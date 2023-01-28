namespace FSH.Core.Persistence;

public interface IWriteRepository<TEntity, in TId> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}
