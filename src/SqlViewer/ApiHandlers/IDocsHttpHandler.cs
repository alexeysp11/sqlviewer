using SqlViewer.Common.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiHandlers;

public interface IDocsHttpHandler : IDisposable
{
    Task<SqlViewerDocsResponseDto> GetDbProviderDocs(VelocipedeDatabaseType databaseType);
}
