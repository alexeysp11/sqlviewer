using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SqlViewer.Constants;
using SqlViewer.Services;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ViewModels;

public class MainViewModel : INotifyPropertyChanged, IDisposable
{
    private string _sqlQuery;
    private string _connectionString;
    private string _sqlCommandLogs;
    private string _selectedRdbms;

    private DataTable _queryResults;

    private readonly SqlApiService _sqlApiService;

    public MainViewModel()
    {
        AvailableRdbms = [DatabaseTypeConstants.Sqlite, DatabaseTypeConstants.Postgres, DatabaseTypeConstants.SqlServer];
        SelectedRdbms = AvailableRdbms[0];

        QuerySqlCommand = new AsyncRelayCommand(QuerySqlAsync, () => !string.IsNullOrWhiteSpace(SqlQuery));
        ExecuteSqlCommand = new RelayCommand(ExecuteSql, () => !string.IsNullOrWhiteSpace(SqlQuery));
        //NewConnectionCommand = new RelayCommand();
        //OpenFileCommand = new RelayCommand();

        _sqlApiService = new SqlApiService();
    }

    public ObservableCollection<string> AvailableRdbms { get; }

    public string SelectedRdbms
    {
        get => _selectedRdbms;
        set { _selectedRdbms = value; OnPropertyChanged(); }
    }

    public string ConnectionString
    {
        get => _connectionString;
        set { _connectionString = value; OnPropertyChanged(); }
    }

    public string SqlQuery
    {
        get => _sqlQuery;
        set { _sqlQuery = value; OnPropertyChanged(); }
    }

    public string SqlCommandLogs
    {
        get => _sqlCommandLogs;
        private set { _sqlCommandLogs = value; OnPropertyChanged(); }
    }

    public DataTable QueryResults
    {
        get => _queryResults;
        set
        {
            _queryResults = value;
            OnPropertyChanged(nameof(QueryResults));
        }
    }

    public IAsyncRelayCommand QuerySqlCommand { get; }
    public ICommand ExecuteSqlCommand { get; }
    public ICommand NewConnectionCommand { get; }
    public ICommand OpenFileCommand { get; }

    private async Task QuerySqlAsync()
    {
        SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Querying data...";

        VelocipedeDatabaseType databaseType = GetDatabaseTypeFromCombo();
        QueryResults = await _sqlApiService.QueryAsync(databaseType, ConnectionString, SqlQuery);

        SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Displaying data";
    }

    private void ExecuteSql()
    {
        SqlCommandLogs += $"\n[{DateTime.Now:HH:mm:ss}] Executing command...";
    }

    private VelocipedeDatabaseType GetDatabaseTypeFromCombo()
    {
        return SelectedRdbms switch
        {
            DatabaseTypeConstants.Sqlite => VelocipedeDatabaseType.SQLite,
            DatabaseTypeConstants.Postgres => VelocipedeDatabaseType.PostgreSQL,
            DatabaseTypeConstants.SqlServer => VelocipedeDatabaseType.MSSQL,
            _ => throw new NotImplementedException("Incorrect database type")
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        _sqlApiService?.Dispose();
    }
}
