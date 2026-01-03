using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public sealed partial class EtlViewModel(
    ISqlApiService sqlApiService,
    IMetadataApiService metadataService,
    IQueryBuilderApiService queryBuilderService) : ObservableRecipient
{
    private enum EtlStepType
    {
        Setup = 0,
        Preview = 1,
    }

    private readonly ISqlApiService _sqlApiService = sqlApiService;
    private readonly IMetadataApiService _metadataService = metadataService;
    private readonly IQueryBuilderApiService _queryBuilderService = queryBuilderService;

    public ObservableCollection<VelocipedeDatabaseType> DatabaseTypes { get; } =
    [
        VelocipedeDatabaseType.None,
        VelocipedeDatabaseType.SQLite,
        VelocipedeDatabaseType.PostgreSQL,
        VelocipedeDatabaseType.MSSQL
    ];
    public ObservableCollection<string> SourceTables { get; } = [];

    [ObservableProperty]
    private VelocipedeDatabaseType _sourceType;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCreateScriptCommand))]
    private string _sourceConnectionString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCreateScriptCommand))]
    private string _selectedTable;

    [ObservableProperty]
    private VelocipedeDatabaseType _targetType;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCreateScriptCommand))]
    private string _targetConnectionString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteMigrationCommand))]
    private string _generatedSql;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCreateScriptCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExecuteMigrationCommand))]
    [NotifyCanExecuteChangedFor(nameof(PrevStepCommand))]
    private int _currentStep = (int)EtlStepType.Setup;

    [RelayCommand]
    private async Task LoadSourceTables()
    {
        try
        {
            IEnumerable<string> tables = await _metadataService.GetTablesAsync(SourceType, SourceConnectionString);
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

    [RelayCommand(CanExecute = nameof(CanGenerate))]
    private async Task GenerateCreateScript()
    {
        try
        {
            IEnumerable<ColumnInfoResponseDto> columns = await _metadataService.GetColumnsAsync(
                databaseType: SourceType,
                connectionString: SourceConnectionString,
                tableName: SelectedTable);

            GeneratedSql = await _queryBuilderService.GetCreateTableQueryAsync(
                databaseType: TargetType,
                tableName: SelectedTable,
                columnInfos: columns);

            CurrentStep = (int)EtlStepType.Preview;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Generation error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void PrevStep()
    {
        CurrentStep = (int)EtlStepType.Setup;
    }

    [RelayCommand(CanExecute = nameof(CanExecuteMigration))]
    private async Task ExecuteMigration()
    {
        try
        {
            await _sqlApiService.QueryAsync(
                databaseType: TargetType,
                connectionString: TargetConnectionString,
                query: GeneratedSql);
            MessageBox.Show("The create table request has been sent to the target database!", "ETL transferring done", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanGenerate() =>
        CurrentStep == (int)EtlStepType.Setup
        && !string.IsNullOrWhiteSpace(SourceConnectionString)
        && !string.IsNullOrWhiteSpace(TargetConnectionString)
        && !string.IsNullOrWhiteSpace(SelectedTable);

    private bool CanGoBack() => CurrentStep > (int)EtlStepType.Setup;

    private bool CanExecuteMigration() => CurrentStep > (int)EtlStepType.Setup && !string.IsNullOrEmpty(GeneratedSql);
}
