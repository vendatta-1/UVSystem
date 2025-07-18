using UVS.Common.Domain;

namespace UVS.Common.Application.Authorization;

public interface IRoleService
{
    Task<Result<RoleResponse>> GetUserRolesAsync(string userId);
}