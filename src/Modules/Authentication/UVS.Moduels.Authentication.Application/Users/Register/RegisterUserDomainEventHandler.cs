using Microsoft.Extensions.Logging;
using UVS.Common.Application.Messaging;
using UVS.Modules.Authentication.Application.Abstractions.Identity;
using UVS.Modules.Authentication.Domain.Users;

namespace UVS.Modules.Authentication.Application.Users.Register;

internal sealed class RegisterUserDomainEventHandler(IIdentityProviderService identityProviderService):
    DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
 
        var mapRoleResult = await identityProviderService.AssignRoleAsync(domainEvent.RoleName, domainEvent.IdentityId, cancellationToken);

        if (mapRoleResult.IsFailure)
        {
            throw new ApplicationException("Could not assign role");
        }
        

    }
}