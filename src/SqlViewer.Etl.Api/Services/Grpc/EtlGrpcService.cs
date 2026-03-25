using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SqlViewer.Etl.Api.BusinessLogic;
using SqlViewer.Etl.Api.Mappings;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.Services.Grpc;

public sealed class EtlGrpcService(
    ILogger<EtlGrpcService> logger,
    ITransferManager transferManager,
    ITransferHistoryManager transferHistoryManager) : EtlTransferService.EtlTransferServiceBase
{
    public override async Task<StartTransferResponse> StartTransfer(
        StartTransferRequest request,
        ServerCallContext context)
    {
        if (request.UserUid is null || !Guid.TryParse(request.UserUid, out Guid userUid))
            throw new InvalidOperationException("ETL transfer is available only to authorized users");

        Guid correlationId = Guid.NewGuid();
        StartTransferRequestDto requestDto = new()
        {
            SourceConnectionString = request.SourceConnectionString,
            TargetConnectionString = request.TargetConnectionString,
            SourceDatabaseType = EtlMapper.MapToVelocipedeDatabaseType(request.SourceDatabaseType),
            TargetDatabaseType = EtlMapper.MapToVelocipedeDatabaseType(request.TargetDatabaseType),
            TableName = request.TableName,
            UserUid = request.UserUid
        };

        // Start transfer.
        string requestJson = JsonSerializer.Serialize(requestDto);
        await transferManager.InitiateTransferAsync(correlationId, userUid, requestJson);

        // Save transfer job.
        await transferHistoryManager.SaveTransferJobHistoryAsync(correlationId, requestDto);

        return new StartTransferResponse
        {
            CorrelationId = correlationId.ToString(),
            CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            Message = "Transfer request accepted and placed in queue."
        };
    }

    public override async Task<GetTransferHistoryResponse> GetTransferHistory(
        GetTransferHistoryRequest request,
        ServerCallContext context)
    {
        try
        {
            if (!Guid.TryParse(request.UserUid, out Guid userUid))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user UID format"));
            }

            Guid? cursorGuid = null;
            if (!string.IsNullOrEmpty(request.CursorCorrelationId) &&
                Guid.TryParse(request.CursorCorrelationId, out Guid parsedGuid))
            {
                cursorGuid = parsedGuid;
            }

            TransferHistoryResponseDto history = await transferHistoryManager.GetHistoryAsync(userUid, cursorGuid, request.Limit);
            GetTransferHistoryResponse response = new()
            {
                CursorCorrelationId = history.CursorCorrelationId?.ToString() ?? string.Empty
            };
            response.Items.AddRange(history.Items.Select(item => new TransferJobGrpcDto
            {
                CorrelationId = item.CorrelationId.ToString(),
                SourceConnectionString = item.SourceConnectionString,
                TargetConnectionString = item.TargetConnectionString,
                SourceDatabaseType = EtlMapper.MapToDatabaseType(item.SourceDatabaseType),
                TargetDatabaseType = EtlMapper.MapToDatabaseType(item.TargetDatabaseType),
                TableName = item.TableName,
                Status = item.Status,
                Time = Timestamp.FromDateTime(DateTime.SpecifyKind(item.Time, DateTimeKind.Utc))
            }));
            return response;
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching transfer history for user {UserUid}", request.UserUid);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}
