using SqlViewer.Common.Dtos.Metadata;

namespace SqlViewer.ApiHandlers;

public interface IMetadataHttpHandler : IDisposable
{
    Task<MetadataColumnsResponseDto> GetColumnsAsync(MetadataRequestDto requestDto);
    Task<MetadataTablesResponseDto> GetTables(MetadataRequestDto requestDto);
}
