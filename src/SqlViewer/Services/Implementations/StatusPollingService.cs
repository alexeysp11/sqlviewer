using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.ViewModels;
using System.Collections.ObjectModel;

namespace SqlViewer.Services.Implementations;

#nullable enable

public sealed class StatusPollingService(IEtlDataTransferService etlApiService) : IStatusPollingService
{
    private CancellationTokenSource? _cts;
    private Task? _pollingTask;
    private bool _isDisposed;

    public void StartPolling(ObservableCollection<TransferTaskViewModel> tasks)
    {
        if (_pollingTask != null) return;

        _cts = new CancellationTokenSource();
        _pollingTask = RunPollingAsync(tasks, _cts.Token);
    }

    public void StopPolling()
    {
        _cts?.Cancel();
    }

    private async Task RunPollingAsync(ObservableCollection<TransferTaskViewModel> tasks, CancellationToken ct)
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
                        IEnumerable<Guid> ids = activeJobs.Select(j => j.CorrelationId);
                        List<TransferStatusResponseDto> results = await etlApiService.GetStatusesAsync(ids, ct);
                        foreach (TransferStatusResponseDto result in results)
                        {
                            TransferTaskViewModel? job = activeJobs.FirstOrDefault(j => j.CorrelationId == result.CorrelationId);
                            job?.ApplyChanges(result);
                        }
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        //logger.LogError(ex, "Error during batch status polling.");
                    }
                }

                await Task.Delay(2000, ct);
            }
        }
        catch (OperationCanceledException)
        {
            //logger.LogInformation("Polling was cancelled.");
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
