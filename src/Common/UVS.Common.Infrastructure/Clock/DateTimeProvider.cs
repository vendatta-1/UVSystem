using UVS.Common.Application.Clock;

namespace UVS.Common.Infrastructure.Clock;

internal sealed class DateTimeProvider:IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}