using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface ISqlApiService : IDisposable
{
    Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query);
}
