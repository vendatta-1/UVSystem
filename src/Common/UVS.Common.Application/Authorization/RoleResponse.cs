
namespace UVS.Common.Application.Authorization;

public sealed record RoleResponse(HashSet< string>  Roles, Guid UserId);
 