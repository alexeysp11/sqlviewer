using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Constants;
using SqlViewer.Helpers;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QuerySqlCommand))]
    [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
    private string _connectionString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QuerySqlCommand))]
    private string _sqlQuery;

    [ObservableProperty]
    private string _sqlCommandLogs;

    [ObservableProperty]
    private string _selectedRdbms;

    [ObservableProperty]
    private DataTable _queryResults;

    private readonly ISqlApiService _sqlApiService;
    private readonly IDocsApiService _docsApiService;
    private readonly IMetadataApiService _metadataApiService;
    private readonly IQueryBuilderApiService _queryBuilderService;
    private readonly IWindowService _windowService;

    public MainViewModel(
        ISqlApiService sqlApiService,
        IDocsApiService docsApiService,
        IMetadataApiService metadataApiService,
        IQueryBuilderApiService queryBuilderApiService,
        IWindowService windowService)
    {
        _sqlApiService = sqlApiService;
        _docsApiService = docsApiService;
        _metadataApiService = metadataApiService;
        _queryBuilderService = queryBuilderApiService;
        _windowService = windowService;

        AvailableRdbms =
        [
            DatabaseTypeConstants.Sqlite,
            DatabaseTypeConstants.Postgres,
            DatabaseTypeConstants.SqlServer,
        ];
        SelectedRdbms = AvailableRdbms.First();
        DatabaseTables = [];

        QuerySqlCommand = new AsyncRelayCommand(QuerySqlAsync, CanExecuteSql);
        ClearLogsCommand = new RelayCommand(ClearLogs);
        ConnectCommand = new AsyncRelayCommand(RefreshMetadataAsync, CanExecuteConnect);
        ExitCommand = new RelayCommand(Exit);
        OpenEtlCommand = new RelayCommand(OpenEtl);
        HelpCommand = new AsyncRelayCommand<string>(GetHelpAsync);
    }

    public ObservableCollection<string> AvailableRdbms { get; }
    public ObservableCollection<TableViewModel> DatabaseTables { get; }

    public IAsyncRelayCommand QuerySqlCommand { get; }
    public IRelayCommand NewConnectionCommand { get; }
    public IRelayCommand OpenFileCommand { get; }
    public IRelayCommand ClearLogsCommand { get; }
    public IAsyncRelayCommand ConnectCommand { get; }
    public IRelayCommand ExitCommand { get; }
    public IRelayCommand OpenEtlCommand { get; }
    public IAsyncRelayCommand<string> HelpCommand { get; }

    private async Task QuerySqlAsync()
    {
        try
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Executing query...";

            VelocipedeDatabaseType databaseType = GetDatabaseTypeFromCombo();
            QueryResults = await _sqlApiService.QueryAsync(databaseType, ConnectionString, SqlQuery);

            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Query executed";
        }
        catch (Exception ex)
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] {ex.Message}";
        }
    }

    private void ClearLogs() => SqlCommandLogs = string.Empty;

    private void Exit()
    {
        try
        {
            string msg = "Are you sure to close the application?";
            if (MessageBox.Show(msg, "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        catch (Exception ex)
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] {ex.Message}";
        }
    }

    private void OpenEtl()
    {
        VelocipedeDatabaseType databaseType = GetDatabaseTypeFromCombo();
        _windowService.ShowEtlWizard(_sqlApiService, _metadataApiService, _queryBuilderService, ConnectionString, databaseType);
    }

    private async Task GetHelpAsync(string parameter)
    {
        try
        {
            if (parameter is HelpCommandParameter.About)
            {
                // TODO: get about page from backend.
                string filePath = DocsHelper.SaveContentAsFile("Hello", "about.html");
                DocsHelper.OpenDocsInBrowser("About info", filePath);
            }
            else
            {
                VelocipedeDatabaseType databaseType = parameter switch
                {
                    HelpCommandParameter.SqliteDocs => VelocipedeDatabaseType.SQLite,
                    HelpCommandParameter.PostgresDocs => VelocipedeDatabaseType.PostgreSQL,
                    HelpCommandParameter.SqlServerDocs => VelocipedeDatabaseType.MSSQL,
                    _ => throw new NotSupportedException($"Unable to convert help command parameter to database type"),
                };
                string url = await _docsApiService.GetDbProviderDocs(databaseType);
                DocsHelper.OpenDocsInBrowser($"{databaseType} documentation", url);
            }
        }
        catch (Exception ex)
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] {ex.Message}";
        }
    }

    public async Task RefreshMetadataAsync()
    {
        try
        {
            DatabaseTables.Clear();
            VelocipedeDatabaseType databaseType = GetDatabaseTypeFromCombo();
            IEnumerable<string> tables = await _metadataApiService.GetTablesAsync(databaseType, ConnectionString);
            foreach (string table in tables)
            {
                DatabaseTables.Add(new TableViewModel
                {
                    Name = table,
                    LoadColumnsFunc = async (tName) =>
                    {
                        try
                        {
                            return await _metadataApiService.GetColumnsAsync(databaseType, ConnectionString, tName);
                        }
                        catch (Exception ex)
                        {
                            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Error loading columns for {tName}: {ex.Message}";
                            return [];
                        }
                    }
                });
            }
        }
        catch (Exception ex)
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] {ex.Message}";
        }
    }

    private VelocipedeDatabaseType GetDatabaseTypeFromCombo() => SelectedRdbms switch
    {
        DatabaseTypeConstants.Sqlite => VelocipedeDatabaseType.SQLite,
        DatabaseTypeConstants.Postgres => VelocipedeDatabaseType.PostgreSQL,
        DatabaseTypeConstants.SqlServer => VelocipedeDatabaseType.MSSQL,
        _ => throw new NotImplementedException("Incorrect database type")
    };

    private bool CanExecuteSql() => !string.IsNullOrWhiteSpace(SqlQuery) && !string.IsNullOrWhiteSpace(ConnectionString);
    private bool CanExecuteConnect() => !string.IsNullOrWhiteSpace(ConnectionString);

    public void Dispose() => _sqlApiService?.Dispose();
}
