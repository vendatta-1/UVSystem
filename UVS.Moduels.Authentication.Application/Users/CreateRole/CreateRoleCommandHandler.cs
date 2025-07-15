using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Modules.Authentication.Application.Users.CreateRole;

public sealed class CreateRoleCommandHandler(IIdentityProviderService identityProvider):ICommandHandler<CreateRoleCommand, string>
{
    public async Task<Result<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await identityProvider.CreateRoleAsync(new RoleModel(request.Name, request.Description), cancellationToken);

        return result;

    }
}