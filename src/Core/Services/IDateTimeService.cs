namespace FSH.Microservices.Core.Services;

public class IDateTimeService : IScopedService
{
    DateTime DateTimeUtcNow { get; }
    DateOnly DateOnlyUtcNow { get; }
}
