using UVS.Common.Application.Authorization;
using UVS.Common.Application.Messaging;

namespace UVS.Modules.Authentication.Application.Users.GetUserRoles;

public sealed record GetUserRolesQuery(string IdnetityId):IQuery<RoleResponse>;