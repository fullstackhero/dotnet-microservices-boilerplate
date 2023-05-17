using MongoDB.Driver;

namespace FSH.Framework.Persistence.Mongo;
public interface IMongoDbContext : IDisposable
{
    IMongoCollection<T> GetCollection<T>(string? name = null);
}