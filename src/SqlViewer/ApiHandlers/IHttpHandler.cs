using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Dtos.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers;

public interface IHttpHandler : IDisposable
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto);

    Task<MetadataColumnsResponseDto> GetColumnsAsync(MetadataRequestDto requestDto);
    Task<MetadataTablesResponseDto> GetTables(MetadataRequestDto requestDto);

    Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType);

    Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto);
}
