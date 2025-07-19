using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Abstractions.Identity;

namespace UVS.Modules.Authentication.Application.Users.Login;

internal sealed class LoginCommandHandler(IIdentityProviderService identityProviderService) :ICommandHandler<LoginCommand, string>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await identityProviderService.LoginAsync(new LoginModel(request.Username, request.Password), cancellationToken);
        
        return result;
    }
}