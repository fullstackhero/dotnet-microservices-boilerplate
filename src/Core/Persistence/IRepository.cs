namespace FSH.Core.Persistence;

public interface IRepository<TEntity, in TId> :
    IReadRepository<TEntity, TId>,
    IWriteRepository<TEntity, TId>,
    IDisposable
    where TEntity : class
{
}
