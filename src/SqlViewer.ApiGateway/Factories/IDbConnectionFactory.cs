using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Factories;

public interface IDbConnectionFactory
{
    IVelocipedeDbConnection GetDbConnection(VelocipedeDatabaseType databaseType, string connectionString);
}
