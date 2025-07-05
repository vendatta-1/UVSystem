using System.Data.Common;

namespace UVS.Modules.System.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
