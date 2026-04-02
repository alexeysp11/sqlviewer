using System.Collections.Immutable;
using System.Collections.ObjectModel;
using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.StorageContexts.Abstractions;
using SqlViewer.ViewModels;

namespace SqlViewer.Services.Implementations;

#nullable enable

public sealed class StatusPollingService(IEtlDataTransferService etlApiService, IUserContext userContext) : IStatusPollingService
{
    private CancellationTokenSource? _cts;
    private Task? _pollingTask;
    private bool _isDisposed;
    private Guid _userUid;

    public void StartPolling(ObservableCollection<TransferTaskViewModel> tasks, ObservableCollection<string> executionLogs)
    {
        if (_pollingTask != null)
            return;

        if (userContext.CurrentUser?.UserUid is null)
            throw new InvalidOperationException("Unable to run data transfer status polling for unauthorized users");
        _userUid = (Guid)userContext.CurrentUser.UserUid;

        _cts = new CancellationTokenSource();
        _pollingTask = RunPollingAsync(tasks, executionLogs, _cts.Token);
    }

    public void StopPolling()
    {
        _cts?.Cancel();
    }

    private async Task RunPollingAsync(
        ObservableCollection<TransferTaskViewModel> tasks,
        ObservableCollection<string> executionLogs,
        CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                List<TransferTaskViewModel> activeJobs = tasks.Where(j => !j.IsCompleted).ToList();
                if (activeJobs.Count > 0)
                {
                    try
                    {
                        BatchTransferStatusesRequestDto requestDto = new()
                        {
                            CorrelationIds = activeJobs.Select(j => j.CorrelationId).ToImmutableList(),
                            UserUid = _userUid
                        };
                        BatchTransferStatusesResponseDto responseDto = await etlApiService.GetStatusesAsync(requestDto, ct);
                        foreach (TransferStatusResponseDto result in responseDto.Items)
                        {
                            TransferTaskViewModel? job = activeJobs.FirstOrDefault(j => j.CorrelationId == result.CorrelationId);
                            job?.ApplyChanges(result);
                        }
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        executionLogs.Add($"[{DateTime.Now:HH:mm:ss}] [Transfer status polling]: {ex.Message}");
                    }
                }

                await Task.Delay(2000, ct);
            }
        }
        catch (OperationCanceledException)
        {
            executionLogs.Add($"[{DateTime.Now:HH:mm:ss}] [Transfer status polling]: Polling was cancelled.");
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _cts?.Cancel();
        _cts?.Dispose();
        _isDisposed = true;
    }
}
