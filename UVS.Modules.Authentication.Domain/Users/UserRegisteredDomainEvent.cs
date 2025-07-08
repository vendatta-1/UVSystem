using UVS.Common.Domain;

namespace UVS.Authentication.Domain.Users;

public sealed class UserRegisteredDomainEvent(Guid userId):DomainEvent
{
    public Guid UserId { get; init; } = userId;

}