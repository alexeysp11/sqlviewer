using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SqlViewer.Metadata.Domain.MetadataRegistries;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.Shared.Extensions;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Metadata.Services.Grpc;

[Authorize]
public sealed class MetadataGrpcService(
    ILogger<MetadataGrpcService> logger,
    IMetadataRegistry metadataRegistry)
    : MetadataService.MetadataServiceBase
{
    public override async Task<MetadataTablesResponse> GetTables(MetadataRequest request, ServerCallContext context)
    {
        try
        {
            List<string> result = await metadataRegistry.GetTablesAsync(
                databaseType: (VelocipedeDatabaseType)request.DatabaseType,
                connectionString: request.ConnectionString);

            MetadataTablesResponse response = new();
            response.Tables.AddRange(result);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to get tables in {DatabaseType}: {Message}", request.DatabaseType, ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<MetadataColumnsResponse> GetColumns(MetadataRequest request, ServerCallContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(request.TableName))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Table name should be specified"));
            }

            List<VelocipedeNativeColumnInfo> result = await metadataRegistry.GetColumnsAsync(
                databaseType: (VelocipedeDatabaseType)request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName);

            IEnumerable<ColumnInfoResponseDto> dtos = result.GetColumnInfoDtos();

            MetadataColumnsResponse response = new()
            {
                TableName = request.TableName,
                DatabaseType = request.DatabaseType
            };

            response.Columns.AddRange(dtos.Select(MapToProto));
            return response;
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            logger.LogError(
                "Unable to get table columns for '{TableName}' in {DatabaseType}: {Message}",
                request.TableName,
                request.DatabaseType,
                ex.Message);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    private static ColumnInfoResponse MapToProto(ColumnInfoResponseDto dto)
    {
        ColumnInfoResponse column = new()
        {
            ColumnName = dto.ColumnName,
            ColumnType = (DbType)dto.ColumnType,
            IsPrimaryKey = dto.IsPrimaryKey,
            IsNullable = dto.IsNullable,
            DefaultValue = MapToValue(dto.DefaultValue)
        };

        if (dto.CharMaxLength.HasValue) column.CharMaxLength = dto.CharMaxLength.Value;
        if (dto.NumericPrecision.HasValue) column.NumericPrecision = dto.NumericPrecision.Value;
        if (dto.NumericScale.HasValue) column.NumericScale = dto.NumericScale.Value;
        if (dto.NativeColumnType != null) column.NativeColumnType = dto.NativeColumnType;

        return column;
    }

    private static Value MapToValue(object? obj)
    {
        return obj switch
        {
            null => Value.ForNull(),
            string s => Value.ForString(s),
            bool b => Value.ForBool(b),
            double d => Value.ForNumber(d),
            float f => Value.ForNumber(f),
            int i => Value.ForNumber(i),
            long l => Value.ForNumber(l),
            _ => Value.ForString(obj.ToString() ?? string.Empty)
        };
    }
}
