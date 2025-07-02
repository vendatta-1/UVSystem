using System.Data.Common;
using Npgsql;
using UVS.Application.Data;

namespace UVS.Infrastructure.Data;

public sealed class DbConnectionFactory(NpgsqlDataSource dataSource):IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}