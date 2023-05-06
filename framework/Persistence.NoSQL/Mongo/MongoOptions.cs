using FSH.Framework.Infrastructure.Options;

namespace FSH.Framework.Persistence.NoSQL.Mongo;

public class MongoOptions : IOptionsRoot
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}

