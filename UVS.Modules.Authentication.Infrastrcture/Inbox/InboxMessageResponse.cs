namespace UVS.Authentication.Infrastructure.Inbox;

public sealed class InboxMessageResponse(Guid  id, string content)
{
    public Guid Id { get; init; } = id;
    public string Content { get; init; } = content;
}