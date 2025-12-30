using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Factories;

namespace SqlViewer.ApiGateway.Factories;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    public IVelocipedeDbConnection GetDbConnection(VelocipedeDatabaseType databaseType, string connectionString)
        => VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, connectionString);
}
