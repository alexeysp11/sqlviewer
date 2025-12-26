using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Constants;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QuerySqlCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSqlCommand))]
    private string _connectionString;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(QuerySqlCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExecuteSqlCommand))]
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
        ExecuteSqlCommand = new RelayCommand(ExecuteSql, CanExecuteSql);

        _sqlApiService = new SqlApiService();
    }

    public ObservableCollection<string> AvailableRdbms { get; }

    public IAsyncRelayCommand QuerySqlCommand { get; }
    public IRelayCommand ExecuteSqlCommand { get; }
    public IRelayCommand NewConnectionCommand { get; }
    public IRelayCommand OpenFileCommand { get; }

    private async Task QuerySqlAsync()
    {
        try
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Querying data...";

            VelocipedeDatabaseType databaseType = GetDatabaseTypeFromCombo();
            QueryResults = await _sqlApiService.QueryAsync(databaseType, ConnectionString, SqlQuery);

            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Displaying data";
        }
        catch (Exception ex)
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] {ex.Message}";
        }
    }

    private void ExecuteSql()
    {
        try
        {
            SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Executing command...";
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
