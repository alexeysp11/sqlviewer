using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Abstractions;

public interface ISqlApiService
{
    Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query);
}
