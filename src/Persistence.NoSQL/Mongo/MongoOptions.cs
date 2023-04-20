using FSH.Microservices.Infrastructure.Options;

namespace FSH.Microservices.Persistence.NoSQL.Mongo;

public class MongoOptions : IOptionsRoot
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}

