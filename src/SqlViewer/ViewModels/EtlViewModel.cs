using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public sealed partial class EtlViewModel(IMetadataApiService metadataService) : ObservableRecipient
{
    private readonly IMetadataApiService _metadataService = metadataService;

    public ObservableCollection<VelocipedeDatabaseType> DatabaseTypes { get; } = [.. Enum.GetValues<VelocipedeDatabaseType>()];
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
    private int _currentStep; // 0 - Setup, 1 - Preview

    [RelayCommand]
    private async Task LoadSourceTables()
    {
        try
        {
            IEnumerable<string> tables = await _metadataService.GetTablesAsync(SourceType, SourceConnectionString);
            SourceTables.Clear();
            foreach (string table in tables)
                SourceTables.Add(table);
        }
        catch (Exception ex)
        {
            GeneratedSql = $"Error loading tables: {ex.Message}";
        }
    }

    [RelayCommand(CanExecute = nameof(CanGenerate))]
    private async Task GenerateCreateScript()
    {
        try
        {
            IEnumerable<ColumnInfoResponseDto> columns = await _metadataService.GetColumnsAsync(
                SourceType,
                SourceConnectionString,
                SelectedTable);

            string sql = "/* Generated SQL here */";

            GeneratedSql = sql;
            CurrentStep = 1;
        }
        catch (Exception ex)
        {
            GeneratedSql = $"Generation error: {ex.Message}";
        }
    }

    private bool CanGenerate() =>
        CurrentStep == 0
        && !string.IsNullOrWhiteSpace(SourceConnectionString)
        && !string.IsNullOrWhiteSpace(TargetConnectionString)
        && !string.IsNullOrWhiteSpace(SelectedTable);

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void PrevStep()
    {
        CurrentStep = 0;
    }

    private bool CanGoBack() => CurrentStep > 0;

    [RelayCommand(CanExecute = nameof(CanExecuteMigration))]
    private async Task ExecuteMigration()
    {
        // await _databaseService.ExecuteNonQueryAsync(TargetType, TargetConnectionString, GeneratedSql);
        MessageBox.Show("The create table request has been sent to the target database!");
        await Task.CompletedTask;
    }

    private bool CanExecuteMigration() => CurrentStep > 0 && !string.IsNullOrEmpty(GeneratedSql);
}
