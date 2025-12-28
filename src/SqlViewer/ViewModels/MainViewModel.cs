using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.ApiHandlers;
using SqlViewer.Constants;
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

        HttpHandler httpHandler = new();
        _sqlApiService = new SqlApiService(httpHandler);
    }

    public ObservableCollection<string> AvailableRdbms { get; }

    public IAsyncRelayCommand QuerySqlCommand { get; }
    public IRelayCommand NewConnectionCommand { get; }
    public IRelayCommand OpenFileCommand { get; }
    public IRelayCommand ClearLogsCommand { get; }

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
