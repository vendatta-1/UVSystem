using UVS.Common.Domain;

namespace UVS.Modules.Authentication.Domain.Users;

public sealed class UserRegisteredDomainEvent(Guid userId,
    string identityId,
    string roleName) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
    public string IdentityId { get; init; } = identityId;
    public string RoleName { get; init; } = roleName;

}