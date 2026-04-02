using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Shared.Factories;

public interface IDbConnectionFactory
{
    Task<IVelocipedeDbConnection> GetDbConnectionAsync(VelocipedeDatabaseType databaseType, string? connectionString = null);
}
