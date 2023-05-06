using FSH.Framework.Infrastructure.Options;

namespace FSH.Framework.Infrastructure.Caching;
public class CachingOptions : IOptionsRoot
{
    public bool EnableDistributedCaching { get; set; } = false;
    public int SlidingExpirationInMinutes { get; set; } = 2;
    public int AbsoluteExpirationInMinutes { get; set; } = 5;
}
