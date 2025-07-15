using Microsoft.Extensions.Logging;
using UVS.Common.Application.Messaging;
using UVS.Modules.Authentication.Application.Abstractions.Identity;
using UVS.Modules.Authentication.Domain.Users;

namespace UVS.Modules.Authentication.Application.Users.Register;

internal sealed class RegisterUserDomainEventHandler(IIdentityProviderService providerService):
    DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken)
    { 
        var id = await providerService.GetRoleIdAsync(Role.Student.Name, cancellationToken);
        
    }
}