using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Mappings;

public static class EtlMapper
{
    public static DatabaseType MapToDatabaseType(VelocipedeDatabaseType source)
    {
        return source switch
        {
            VelocipedeDatabaseType.None => DatabaseType.None,
            VelocipedeDatabaseType.InMemory => DatabaseType.InMemory,
            VelocipedeDatabaseType.Oracle => DatabaseType.Oracle,
            VelocipedeDatabaseType.Clickhouse => DatabaseType.Clickhouse,
            VelocipedeDatabaseType.Firebird => DatabaseType.Firebird,
            VelocipedeDatabaseType.SQLite => DatabaseType.Sqlite,
            VelocipedeDatabaseType.PostgreSQL => DatabaseType.Postgresql,
            VelocipedeDatabaseType.MSSQL => DatabaseType.Mssql,
            VelocipedeDatabaseType.MySQL => DatabaseType.Mysql,
            VelocipedeDatabaseType.MariaDB => DatabaseType.Mariadb,
            VelocipedeDatabaseType.HSQLDB => DatabaseType.Hsqldb,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, $"The value of enum {nameof(VelocipedeDatabaseType)} is not supported"),
        };
    }

    public static VelocipedeDatabaseType MapToVelocipedeDatabaseType(DatabaseType source)
    {
        return source switch
        {
            DatabaseType.None => VelocipedeDatabaseType.None,
            DatabaseType.InMemory => VelocipedeDatabaseType.InMemory,
            DatabaseType.Oracle => VelocipedeDatabaseType.Oracle,
            DatabaseType.Clickhouse => VelocipedeDatabaseType.Clickhouse,
            DatabaseType.Firebird => VelocipedeDatabaseType.Firebird,
            DatabaseType.Sqlite => VelocipedeDatabaseType.SQLite,
            DatabaseType.Postgresql => VelocipedeDatabaseType.PostgreSQL,
            DatabaseType.Mssql => VelocipedeDatabaseType.MSSQL,
            DatabaseType.Mysql => VelocipedeDatabaseType.MySQL,
            DatabaseType.Mariadb => VelocipedeDatabaseType.MariaDB,
            DatabaseType.Hsqldb => VelocipedeDatabaseType.HSQLDB,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, $"The value of enum {nameof(DatabaseType)} is not supported"),
        };
    }
}
