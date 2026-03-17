using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SqlViewer.Services;
using SqlViewer.Common.Dtos.Etl;
using VelocipedeUtils.Shared.DbOperations.Enums;
using SqlViewer.Models;

namespace SqlViewer.ViewModels;

public partial class EtlDataTransferViewModel(IEtlDataTransferService etlService) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TransferTaskViewModel> _activeTransfers = [];

    [ObservableProperty]
    private ObservableCollection<string> _executionLogs = [];

    [ObservableProperty]
    private VelocipedeDatabaseType _sourceType;

    [ObservableProperty]
    private string _sourceConnectionString;

    [ObservableProperty]
    private string _targetConnectionString;

    [ObservableProperty]
    private string _selectedTableName;

    [ObservableProperty]
    private TransferTask _selectedTransfer;

    partial void OnSelectedTransferChanged(TransferTask value)
    {
        if (value is not null)
        {
            // Download logs for a specific saga.
            //LoadLogsForCorrelationId(value.CorrelationId);
        }
    }

    [RelayCommand]
    private async Task StartTransfer()
    {
        if (string.IsNullOrWhiteSpace(SourceConnectionString) ||
            string.IsNullOrWhiteSpace(TargetConnectionString) ||
            string.IsNullOrWhiteSpace(SelectedTableName))
        {
            ExecutionLogs.Insert(0, "[Error]: Connection strings and table name are required.");
            return;
        }

        try
        {
            StartTransferRequestDto request = new()
            {
                SourceConnectionString = SourceConnectionString,
                TargetConnectionString = TargetConnectionString,
                TableName = SelectedTableName
            };
            Guid correlationId = await etlService.StartTransferAsync(request);

            // Add to the list of active tasks for the UI.
            TransferTaskViewModel task = new()
            {
                CorrelationId = correlationId,
                TableName = SelectedTableName,
                Status = "Initializing...",
                Progress = 0
            };

            ActiveTransfers.Add(task);
            ExecutionLogs.Insert(0, $"[{DateTime.Now:HH:mm:ss}] Task created. ID: {correlationId}");

            _ = PollStatus(task);
        }
        catch (Exception ex)
        {
            ExecutionLogs.Insert(0, $"[Launch error]: {ex.Message}");
        }
    }


    private async Task PollStatus(TransferTaskViewModel task)
    {
        try
        {
            while (!task.IsCompleted)
            {
                await Task.Delay(2000);

                // Receive up-to-date data from the service
                TransferStatusResponseDto result = await etlService.GetStatusAsync(task.CorrelationId);

                // Updating task properties
                task.Progress = result.Progress;
                task.Status = result.StatusMessage;

                // Key point: we set IsCompleted from the DTO,
                // which came from the Saga State Machine via the API Gateway
                task.IsCompleted = result.IsFinalState;
                if (task.Status.Contains("Failed", StringComparison.OrdinalIgnoreCase))
                {
                    task.IsCompleted = true; // Forcefully terminate the survey on error
                }

                if (task.IsCompleted) break;
            }
        }
        catch (Exception ex)
        {
            task.Status = $"Error: {ex.Message}";
            task.IsCompleted = true;
        }
    }
}
