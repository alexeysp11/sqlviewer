using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Models;
using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.StorageContexts.Abstractions;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public partial class DataTransferViewModel(
    IEtlDataTransferService etlService,
    IStatusPollingService pollingService,
    IMetadataApiService metadataService,
    IUserContext userContext) : ObservableObject, IDisposable
{
    public ObservableCollection<string> SourceTables { get; } = [];
    public ObservableCollection<TransferJobDto> TransferHistory { get; } = [];

    public ObservableCollection<VelocipedeDatabaseType> DatabaseTypes { get; } =
    [
        VelocipedeDatabaseType.None,
        VelocipedeDatabaseType.SQLite,
        VelocipedeDatabaseType.PostgreSQL,
        VelocipedeDatabaseType.MSSQL
    ];

    [ObservableProperty]
    private ObservableCollection<TransferTaskViewModel> _activeTransfers = [];

    [ObservableProperty]
    private ObservableCollection<string> _executionLogs = [];

    [ObservableProperty]
    private VelocipedeDatabaseType _sourceType;

    [ObservableProperty]
    private string _sourceConnectionString;

    [ObservableProperty]
    private string _selectedTableName;

    [ObservableProperty]
    private VelocipedeDatabaseType _targetType;

    [ObservableProperty]
    private string _targetConnectionString;

    [ObservableProperty]
    private TransferTask _selectedTransfer;

    [ObservableProperty]
    private bool _isLoadingHistory;

    /// <summary>
    /// Transfer job limit.
    /// </summary>
    private const int Limit = 25;

    [RelayCommand]
    private async Task LoadSourceTables()
    {
        try
        {
            IEnumerable<string> tables = await metadataService.GetTablesAsync(SourceType, SourceConnectionString);
            SourceTables.Clear();
            foreach (string table in tables)
            {
                SourceTables.Add(table);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        if (userContext.CurrentUser.UserUid is null)
        {
            ExecutionLogs.Insert(0, "[Error]: ETL transfer is only available to authorized users.");
            return;
        }

        try
        {
            StartTransferRequestDto request = new()
            {
                SourceConnectionString = SourceConnectionString,
                TargetConnectionString = TargetConnectionString,
                SourceDatabaseType = SourceType,
                TargetDatabaseType = TargetType,
                TableName = SelectedTableName,
                UserUid = userContext.CurrentUser.UserUid!.ToString(),
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
        }
        catch (Exception ex)
        {
            ExecutionLogs.Insert(0, $"[Launch error]: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task LoadMoreHistoryAsync()
    {
        if (IsLoadingHistory)
            return;

        IsLoadingHistory = true;
        try
        {
            if (userContext?.CurrentUser?.UserUid is null)
                throw new InvalidOperationException("Data transfer history is only available to authorized users.");

            // TODO: Pass null as a cursor, since pagination does not work correctly on the backend.
            TransferHistoryResponseDto response = await etlService.GetHistoryAsync(
                userUid: (Guid)userContext.CurrentUser.UserUid,
                cursorTransferJobId: null,
                limit: Limit);

            TransferHistory.Clear();
            foreach (TransferJobDto item in response.Items)
                TransferHistory.Add(item);
        }
        catch (Exception ex)
        {
            ExecutionLogs.Insert(0, $"[History error]: {ex.Message}");
        }
        finally
        {
            IsLoadingHistory = false;
        }
    }

    public void OnViewLoaded()
    {
        try
        {
            pollingService.StartPolling(ActiveTransfers);
        }
        catch (Exception ex)
        {
            ExecutionLogs.Insert(0, $"[Initialization error]: {ex.Message}");
        }
    }

    public void Dispose()
    {
        pollingService.StopPolling();
        pollingService.Dispose();
    }
}
