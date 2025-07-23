using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using UVS.Common.Application.Authorization;
using UVS.Common.Application.Exceptions;
using UVS.Common.Domain;
using UVS.Common.Infrastructure.Authentication;

namespace UVS.Common.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation(IServiceScopeFactory scopeFactory)
    : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
        {
            return principal;
        }
        using var scope = scopeFactory.CreateScope();
        
        IRoleService roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

        string identityId = principal.GetIdentityId();

        Result<RoleResponse> result = await roleService.GetUserRolesAsync(identityId);

        if (result.IsFailure)
        {
            throw new UVSException(nameof(IRoleService.GetUserRolesAsync), result.Error);
        }

        var claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));

        foreach (var role in result.Value.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}