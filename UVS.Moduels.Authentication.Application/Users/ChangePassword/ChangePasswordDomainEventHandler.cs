using UVS.Common.Application.Messaging;
using UVS.Modules.Authentication.Domain.Users;

namespace UVS.Modules.Authentication.Application.Users.ChangePassword;

internal sealed class ChangePasswordDomainEventHandler:DomainEventHandler<ChangePasswordDomainEvent>
{
    public override Task Handle(ChangePasswordDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}