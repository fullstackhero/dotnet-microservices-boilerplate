using FSH.Core.Persistence;

namespace Persistence.MongoDb;

public interface IMongoRepository<TEntity, in TId> : IRepository<TEntity, TId> where TEntity : class
{
}

