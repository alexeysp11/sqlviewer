using SqlViewer.Shared.Dtos.Metadata;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IMetadataHttpHandler
{
    Task<MetadataColumnsResponseDto> GetColumnsAsync(MetadataRequestDto requestDto, CancellationToken ct);
    Task<MetadataTablesResponseDto> GetTables(MetadataRequestDto requestDto, CancellationToken ct);
}
