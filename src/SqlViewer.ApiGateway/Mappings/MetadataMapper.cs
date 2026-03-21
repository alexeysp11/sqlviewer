using Google.Protobuf.WellKnownTypes;
using Riok.Mapperly.Abstractions;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.Metadata;
using SqlViewer.QueryBuilder;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Mappings;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "<Pending>")]
public static partial class MetadataMapper
{
    [MapProperty(nameof(MetadataRequestDto.DatabaseType), nameof(MetadataRequest.DatabaseType), Use = nameof(MapToDatabaseType))]
    [MapProperty(nameof(MetadataRequestDto.TableName), nameof(MetadataRequest.TableName), Use = nameof(MapString))]
    public static partial MetadataRequest MapToProto(this MetadataRequestDto dto);

    public static partial CreateTableRequest MapToProto(this CreateTableRequestDto dto);

    public static partial MetadataTablesResponseDto MapToDto(this MetadataTablesResponse proto);

    [MapProperty(nameof(MetadataRequest.DatabaseType), nameof(MetadataRequestDto.DatabaseType), Use = nameof(MapToVelocipedeDatabaseType))]
    public static partial MetadataColumnsResponseDto MapToDto(this MetadataColumnsResponse proto);

    private static DatabaseType MapToDatabaseType(VelocipedeDatabaseType source)
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

    private static VelocipedeDatabaseType MapToVelocipedeDatabaseType(DatabaseType source)
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

    private static object? MapValue(Value value) => value.KindCase switch
    {
        Value.KindOneofCase.StringValue => value.StringValue,
        Value.KindOneofCase.NumberValue => value.NumberValue,
        Value.KindOneofCase.BoolValue => value.BoolValue,
        Value.KindOneofCase.NullValue => null,
        _ => value.ToString()
    };

    private static Value MapObject(object? obj) => obj == null
        ? Value.ForNull()
        : Value.ForString(obj.ToString());

    private static string MapString(string? value) => value ?? string.Empty;
}
