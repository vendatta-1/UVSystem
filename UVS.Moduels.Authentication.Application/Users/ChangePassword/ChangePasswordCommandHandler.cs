using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Modules.Authentication.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandHandler(IIdentityProviderService identityProviderService):ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
         var result = await identityProviderService.ChangePasswordAsync(new ChangePasswordModel(request.Username, request.OldPassword, request.NewPassword),  cancellationToken);
         
         return result;
    }
}