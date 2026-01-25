using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class DocsApiService(IDocsHttpHandler httpHandler) : IDocsApiService
{
    public async Task<string> GetDbProviderDocs(VelocipedeDatabaseType databaseType)
    {
        SqlViewerDocsResponseDto responseDto = await httpHandler.GetDbProviderDocs(databaseType);
        if (string.IsNullOrEmpty(responseDto.Url))
            throw new InvalidOperationException("Received empty URL for the specified database type");
        return responseDto.Url;
    }

    public void Dispose() => httpHandler?.Dispose();
}
