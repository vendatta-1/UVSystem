using System.Data.Common;
using Dapper;
using UVS.Common.Application.Authorization;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.Authentication.Application.Users.GetUserRoles;

internal sealed class GetUserRolesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    :IQueryHandler<GetUserRolesQuery, RoleResponse>
{
    public  async Task<Result<RoleResponse>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql = $"""
                            SELECT 
                                   ur.role_name AS {nameof(UserRole.RoleName)},
                                   u.id AS {nameof(UserRole.UserId)}
                            FROM auth.users as u
                            JOIN auth.user_roles ur on u.id = ur.user_id
                            where u.identity_id = @IdentityId
                            """;
        List<UserRole>  roles = (await dbConnection.QueryAsync<UserRole>(sql, new {IdentityId = request.IdnetityId})).ToList();

        if (roles.Count == 0)
        {
            const string keySql = $"""
                                  
                                  select kr."name" AS {nameof(UserRole.RoleName)},
                                         ue.id AS {nameof(UserRole.UserId)}
                                    from keycloak.user_entity ue 
                                    join keycloak.user_role_mapping urm on urm.user_id = ue.id 
                                    join keycloak.keycloak_role kr on kr.id = urm.role_id
                                    where ue.id = @IdentityId  
                                """;
            roles = (await dbConnection.QueryAsync<UserRole>(keySql, new {IdentityId = request.IdnetityId})).ToList();
        }
        if (roles.Count == 0)
        {
            return Result.Failure<RoleResponse>(Error.NotFound("Role.NotFound", $"the user with {request.IdnetityId} has no exist roles"));
        }
        
        return Result.Success(new RoleResponse(roles.Select(role => role.RoleName).ToHashSet(),Guid.Parse( roles[0].UserId)));
        
    }

    internal class UserRole
    {
        public string RoleName { get; set; }
        public string UserId { get; set; }
    }
}
 