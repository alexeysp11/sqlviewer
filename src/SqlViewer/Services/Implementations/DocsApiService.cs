using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class DocsApiService(IDocsHttpHandler httpHandler) : IDocsApiService
{
    public async Task<string> GetDbProviderDocs(VelocipedeDatabaseType databaseType)
    {
        SqlViewerDocsResponseDto responseDto = await httpHandler.GetDbProviderDocs(databaseType);

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);
        if (string.IsNullOrEmpty(responseDto.Url))
            throw new InvalidOperationException("Received empty URL for the specified database type");

        return responseDto.Url;
    }

    public void Dispose() => httpHandler?.Dispose();
}
