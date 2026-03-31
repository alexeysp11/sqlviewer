using SqlViewer.Shared.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IDocsHttpHandler
{
    Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType, CancellationToken ct);
}
