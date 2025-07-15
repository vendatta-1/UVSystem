using System.Data.Common;
using Dapper;
using UVS.Authentication.Domain.Users;
using UVS.Common.Application.Messaging;
using UVS.Common.Domain;
using UVS.Modules.System.Application.Data;

namespace UVS.Modules.Authentication.Application.Users.GetUser;

internal sealed class GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(UserResponse.Id)},
                 email AS {nameof(UserResponse.Email)},
                 first_name AS {nameof(UserResponse.FirstName)},
                 last_name AS {nameof(UserResponse.LastName)}
             FROM auth.users
             WHERE id = @UserId
             """;

        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        }

        return user;
    }
}
