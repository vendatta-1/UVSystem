namespace UVS.Authentication.Infrastructure.Outbox;

internal sealed class OutboxMessageResponse(Guid id, string content)
{
    public Guid Id { get; init; } = id;
    public string Content { get; init; } = content;
}