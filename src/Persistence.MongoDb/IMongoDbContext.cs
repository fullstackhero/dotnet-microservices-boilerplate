using MongoDB.Driver;

namespace FSH.Persistence.MongoDb;

public interface IMongoDbContext : IDisposable
{
    IMongoCollection<T> GetCollection<T>(string? name = null);
}
