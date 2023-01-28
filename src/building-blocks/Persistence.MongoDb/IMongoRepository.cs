using FSH.Core.Persistence;

namespace FSH.Persistence.MongoDb;

public interface IMongoRepository<TEntity, in TId> : IRepository<TEntity, TId> where TEntity : class
{
}