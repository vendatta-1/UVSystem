using MediatR;
using UVS.Common.Application.Authorization;
using UVS.Common.Domain;
using UVS.Modules.Authentication.Application.Users.GetUserRoles;

namespace UVS.Modules.Authentication.Infrastructure.Authorization;

internal sealed class RoleService(ISender sender) : IRoleService
{
    public async Task<Result<RoleResponse>> GetUserRolesAsync(string userId)
    {
         return await sender.Send(new GetUserRolesQuery(userId));
    }
}