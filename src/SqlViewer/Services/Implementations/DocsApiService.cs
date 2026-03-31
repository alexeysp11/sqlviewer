using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class DocsApiService(IDocsHttpHandler httpHandler) : IDocsApiService
{
    public async Task<string> GetDbProviderDocs(VelocipedeDatabaseType databaseType)
    {
        SqlViewerDocsResponseDto responseDto = await httpHandler.GetDbProviderDocs(databaseType, default);
        if (string.IsNullOrEmpty(responseDto.Url))
            throw new InvalidOperationException("Received empty URL for the specified database type");
        return responseDto.Url;
    }
}
