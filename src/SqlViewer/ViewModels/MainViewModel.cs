using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SqlViewer.Infrastructure;

namespace SqlViewer.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _sqlQuery;
    private string _connectionString;
    private string _sqlCommandLogs;
    private string _selectedRdbms;

    public MainViewModel()
    {
        AvailableRdbms = ["SQLite", "PostgreSQL", "SQL Server"];
        SelectedRdbms = AvailableRdbms[0];

        QuerySqlCommand = new RelayCommand(o => QuerySql(), c => !string.IsNullOrWhiteSpace(SqlQuery));
        ExecuteSqlCommand = new RelayCommand(o => ExecuteSql(), c => !string.IsNullOrWhiteSpace(SqlQuery));
        NewConnectionCommand = new RelayCommand(o => { });
        OpenFileCommand = new RelayCommand(o => { });
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

    public object QueryResults { get; set; }

    public ICommand QuerySqlCommand { get; }
    public ICommand ExecuteSqlCommand { get; }
    public ICommand NewConnectionCommand { get; }
    public ICommand OpenFileCommand { get; }

    private void QuerySql()
    {
        SqlCommandLogs += $"\n[{System.DateTime.Now:HH:mm:ss}] Querying data...";
    }

    private void ExecuteSql()
    {
        SqlCommandLogs += $"\n[{System.DateTime.Now:HH:mm:ss}] Executing command...";
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
