using FSH.Framework.Core.Services;

namespace FSH.Framework.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow);
}
