using FSH.Microservices.Core.Services;

namespace FSH.Microservices.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow);
}
