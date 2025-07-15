namespace UVS.Authentication.Infrastructure.Inbox;

public sealed class InboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}