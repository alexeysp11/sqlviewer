using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Dtos.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers;

public interface IHttpHandler : IDisposable
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto);
    Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType);
}
