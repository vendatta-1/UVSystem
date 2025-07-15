namespace UVS.Modules.System.Infrastructure.Inbox;

public sealed class InboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}