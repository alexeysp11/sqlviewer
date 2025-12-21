using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using SqlViewer.Models.DbPreproc;
using SqlViewer.Models.DbTransfer;
using SqlViewer.Helpers;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Models.DataStorage;
using SqlViewer.Common.Enums;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms;
using Npgsql;

namespace SqlViewer.ViewModels;

public sealed class DataVM
{
    private MainVM MainVM { get; set; }

    public IDbPreproc AppRdbmsPreproc { get; private set; }
    public IDbPreproc UserRdbmsPreproc { get; private set; }

    public DbInterconnection DbInterconnection { get; private set; }

    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    public DataVM(MainVM mainVM)
    {
        MainVM = mainVM;
        AppRdbmsPreproc = new SqliteDbPreproc(MainVM);
        DbInterconnection = new DbInterconnection();
    }

    public void CreateDb()
    {
        UserRdbmsPreproc.CreateDb();
    }

    public void OpenDb()
    {
        InitUserDbConnection();
        UserRdbmsPreproc.OpenDb();
    }

    public static string GetSqlRequest(string filename)
    {
        return File.ReadAllText($"{SettingsHelper.RootFolder}\\Queries\\{filename}");
    }

    public void InitUserDbConnection()
    {
        switch (RepoHelper.AppSettingsRepo.ActiveRdbms)
        {
            case RdbmsEnum.SQLite:
                (UserRdbmsPreproc = new SqliteDbPreproc(MainVM)).InitUserDbConnection();
                break;

            case RdbmsEnum.PostgreSQL:
                (UserRdbmsPreproc = new PgDbPreproc(MainVM)).InitUserDbConnection();
                break;

            case RdbmsEnum.MySQL:
                (UserRdbmsPreproc = new MysqlDbPreproc(MainVM)).InitUserDbConnection();
                break;

            case RdbmsEnum.MSSQL:
                (UserRdbmsPreproc = new MssqlDbPreproc(MainVM)).InitUserDbConnection();
                break;

            case RdbmsEnum.Oracle:
                (UserRdbmsPreproc = new OracleDbPreproc(MainVM)).InitUserDbConnection();
                break;

            default:
                throw new Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.ActiveRdbms}.");
        }
    }

    public void DisplayTablesInDb()
    {
        UserRdbmsPreproc.DisplayTablesInDb();
    }

    public void GetAllDataFromTable(string tableName)
    {
        UserRdbmsPreproc.GetAllDataFromTable(tableName);
    }

    public void GetColumnsOfTable(string tableName)
    {
        UserRdbmsPreproc.GetColumnsOfTable(tableName);
    }

    public void GetForeignKeys(string tableName)
    {
        UserRdbmsPreproc.GetForeignKeys(tableName);
    }

    public void GetTriggers(string tableName)
    {
        UserRdbmsPreproc.GetTriggers(tableName);
    }

    public void GetSqlDefinition(string tableName)
    {
        UserRdbmsPreproc.GetSqlDefinition(tableName);
    }

    public static async Task<DataTable> QueryAsync(string connectionString, string query)
    {
        SqlQueryRequestDto requestDto = new()
        {
            DatabaseType = RepoHelper.AppSettingsRepo.ActiveRdbms switch
            {
                RdbmsEnum.SQLite => VelocipedeDatabaseType.SQLite,
                RdbmsEnum.PostgreSQL => VelocipedeDatabaseType.PostgreSQL,
                RdbmsEnum.MSSQL => VelocipedeDatabaseType.MSSQL,
                _ => throw new NotImplementedException()
            },
            ConnectionString = connectionString,
            Query = query
        };
        string url = "http://localhost:5293/api/sql/query";
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, requestDto).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        SqlQueryResponseDto responseDto = JsonConvert.DeserializeObject<SqlQueryResponseDto>(jsonResponse);
        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

        return responseDto.QueryResult.ToDataTable();
    }

    public static string GetConnectionStringFromSettings(AppSettingsRepo settingsRepo)
    {
        switch (settingsRepo.ActiveRdbms)
        {
            case RdbmsEnum.SQLite:
                SqliteConnectionStringBuilder sqliteBuilder = [];
                sqliteBuilder.DataSource = settingsRepo.DbName;
                return sqliteBuilder.ConnectionString;

            case RdbmsEnum.PostgreSQL:
                NpgsqlConnectionStringBuilder postgresBuilder = new()
                {
                    Host = settingsRepo.DbHost,
                    Port = 5432,
                    Database = settingsRepo.DbName,
                    Username = settingsRepo.DbUsername,
                    Password = settingsRepo.DbPassword,
                    SslMode = SslMode.Allow,
                };
                return postgresBuilder.ConnectionString;

            default:
                throw new NotImplementedException();
        }
    }

    public void SendSqlRequest()
    {
        string connectionString = GetConnectionStringFromSettings(RepoHelper.AppSettingsRepo);
        string sql = MainVM.MainWindow.SqlPage.mtbSqlRequest.Text;

        DataTable dtResult = QueryAsync(connectionString, sql)
            .GetAwaiter()
            .GetResult();

        MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = dtResult.DefaultView;
        MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible;
        MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true;
    }

    public DataTable SendSqlRequest(string sql)
    {
        DataTable dt = new();
        try
        {
            dt = AppRdbmsPreproc.SendSqlRequest(sql);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return dt;
    }

    public void ClearTempTable(string tableName)
    {
        AppRdbmsPreproc.ClearTempTable(tableName);
    }
}
