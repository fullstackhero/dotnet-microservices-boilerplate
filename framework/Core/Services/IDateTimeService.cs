namespace FSH.Microservices.Core.Services;

public interface IDateTimeService : IScopedService
{
    public DateTime DateTimeUtcNow { get; }
    public DateOnly DateOnlyUtcNow { get; }
}
