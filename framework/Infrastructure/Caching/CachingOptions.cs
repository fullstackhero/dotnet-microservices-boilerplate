using FSH.Microservices.Infrastructure.Options;

namespace FSH.Microservices.Infrastructure.Caching;
public class CachingOptions : IOptionsRoot
{
    public bool EnableDistributedCaching { get; set; } = false;
    public int SlidingExpirationInMinutes { get; set; } = 2;
    public int AbsoluteExpirationInMinutes { get; set; } = 5;
}
