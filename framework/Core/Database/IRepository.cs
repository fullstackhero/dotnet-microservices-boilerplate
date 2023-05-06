using System.Linq.Expressions;

namespace FSH.Framework.Core.Database;

public interface IRepository<TDocument, in TId> : IReadRepository<TDocument, TId>, IWriteRepository<TDocument, TId>, IDisposable where TDocument : class
{
}

public interface IReadRepository<TDocument, in TId> where TDocument : class
{
    Task<TDocument?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);
}

public interface IWriteRepository<TDocument, in TId> where TDocument : class
{
    Task AddAsync(TDocument entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TDocument entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IReadOnlyList<TDocument> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TDocument entity, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}
