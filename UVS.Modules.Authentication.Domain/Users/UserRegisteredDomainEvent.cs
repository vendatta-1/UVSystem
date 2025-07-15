using UVS.Common.Domain;

namespace UVS.Modules.Authentication.Domain.Users;

public sealed class UserRegisteredDomainEvent(Guid userId, Guid identityId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
    public Guid IdentityId { get; init; } = identityId;

}