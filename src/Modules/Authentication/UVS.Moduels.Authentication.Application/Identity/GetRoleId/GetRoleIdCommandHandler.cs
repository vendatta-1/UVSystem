using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Modules.Authentication.Application.Identity.GetRoleId;

internal sealed class GetRoleIdCommandHandler (IIdentityProviderService identityProviderService): ICommandHandler<GetRoleIdCommand,string>
{
    public async Task<Result<string>> Handle(GetRoleIdCommand request, CancellationToken cancellationToken)
    { 
        var result = await identityProviderService.GetRoleIdAsync(request.RoleName, cancellationToken);

        return result;
    }
}