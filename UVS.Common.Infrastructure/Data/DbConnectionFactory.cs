using System.Data.Common;
using Npgsql;
using UVS.Modules.System.Application.Data;

namespace UVS.Common.Infrastructure.Data;

public sealed class DbConnectionFactory(NpgsqlDataSource dataSource):IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}