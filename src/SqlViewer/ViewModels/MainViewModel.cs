using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.ApiHandlers;
using SqlViewer.Constants;
using SqlViewer.Helpers;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QuerySqlCommand))]
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

    private readonly SqlApiService _sqlApiService;
    private readonly DocsApiService _docsApiService;

    public MainViewModel()
    {
        AvailableRdbms =
        [
            DatabaseTypeConstants.Sqlite,
            DatabaseTypeConstants.Postgres,
            DatabaseTypeConstants.SqlServer,
        ];
        SelectedRdbms = AvailableRdbms.First();

        QuerySqlCommand = new AsyncRelayCommand(QuerySqlAsync, CanExecuteSql);
        ClearLogsCommand = new RelayCommand(ClearLogs);
        ExitCommand = new RelayCommand(Exit);
        HelpCommand = new AsyncRelayCommand<string>(GetHelpAsync);

        HttpHandler httpHandler = new();
        _sqlApiService = new SqlApiService(httpHandler);
        _docsApiService = new DocsApiService(httpHandler);
    }

    public ObservableCollection<string> AvailableRdbms { get; }

    public IAsyncRelayCommand QuerySqlCommand { get; }
    public IRelayCommand NewConnectionCommand { get; }
    public IRelayCommand OpenFileCommand { get; }
    public IRelayCommand ClearLogsCommand { get; }
    public IRelayCommand ExitCommand { get; }
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

    private VelocipedeDatabaseType GetDatabaseTypeFromCombo() => SelectedRdbms switch
    {
        DatabaseTypeConstants.Sqlite => VelocipedeDatabaseType.SQLite,
        DatabaseTypeConstants.Postgres => VelocipedeDatabaseType.PostgreSQL,
        DatabaseTypeConstants.SqlServer => VelocipedeDatabaseType.MSSQL,
        _ => throw new NotImplementedException("Incorrect database type")
    };

    private bool CanExecuteSql() => !string.IsNullOrWhiteSpace(SqlQuery) && !string.IsNullOrWhiteSpace(ConnectionString);

    public void Dispose() => _sqlApiService?.Dispose();
}
