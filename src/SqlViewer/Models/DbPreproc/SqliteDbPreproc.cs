using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SqlViewer.Models.DbConnections;
using SqlViewer.Helpers;
using SqlViewer.Views;
using SqlViewer.ViewModels;
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection;
using UserControlsMenu = SqlViewer.UserControls.Menu;
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms;

namespace SqlViewer.Models.DbPreproc;

public class SqliteDbPreproc(MainVM mainVM) : IDbPreproc
{
    private MainVM MainVM { get; set; } = mainVM;

    public ICommonDbConnectionSV AppDbConnection { get; private set; } = new SqliteDbConnection($"{SettingsHelper.RootFolder}\\data\\app.db");
    public ICommonDbConnectionSV UserDbConnection { get; private set; }

    public DataTable ResultCollection { get; private set; }
    public List<string> TablesCollection { get; private set; }

    public ICommonDbConnectionSV GetAppDbConnection()
    {
        return AppDbConnection;
    }
    public ICommonDbConnectionSV GetUserDbConnection()
    {
        return UserDbConnection;
    }

    /// <summary>
    /// Creates a local DB 
    /// </summary>
    public void CreateDb()
    {
        SaveFileDialog sfd = new()
        {
            Filter = SettingsHelper.FilterFileSystemDb
        };
        if (sfd.ShowDialog() == true)
            System.IO.File.WriteAllText(sfd.FileName, string.Empty);
    }

    /// <summary>
    /// Opens a local DB using OpenFileDialog 
    /// </summary>
    public void OpenDb()
    {
        OpenFileDialog ofd = new()
        {
            Filter = SettingsHelper.FilterFileSystemDb
        };
        if (ofd.ShowDialog() == true) { }

        string path = ofd.FileName;
        if (path == string.Empty) return;
        InitLocalDbConnection(path);
        DisplayTablesInDb();
    }

    public void InitUserDbConnection()
    {
        if (RepoHelper.AppSettingsRepo == null)
            throw new Exception("RepoHelper.AppSettingsRepo is not assigned.");
        if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.SQLite)
            throw new Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.ActiveRdbms}.");

        if (!string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName))
            UserDbConnection = new SqliteDbConnection(RepoHelper.AppSettingsRepo.DbName);
    }

    /// <summary>
    /// Initializes DbConnection after OpenFileDialog 
    /// </summary>
    private void InitLocalDbConnection(string path)
    {
        UserDbConnection = new SqliteDbConnection(path);
        MainVM.MainWindow.SqlPage.tblDbName.Text = path;
        MainVM.MainWindow.TablesPage.tblDbName.Text = path;

        if (MainVM.VisualVM.SettingsView != null)
            ((SettingsView)MainVM.VisualVM.SettingsView).tbDatabase.Text = path;
        if (MainVM.VisualVM.LoginView != null)
            ((LoginView)MainVM.VisualVM.LoginView).tbDatabase.Text = path;

        RepoHelper.AppSettingsRepo.SetDbName(path);

        if (MainVM.VisualVM.Menu != null)
            ((UserControlsMenu)MainVM.VisualVM.Menu).Init();
    }

    public void DisplayTablesInDb()
    {
        if (UserDbConnection == null)
        {
            return;
        }

        string sqlRequest = @"
SELECT name FROM sqlite_master
WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%'
UNION ALL
SELECT name FROM sqlite_temp_master
WHERE type IN ('table','view')
ORDER BY 1";
        DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
        MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            TreeViewItem item = new()
            {
                Header = row["name"].ToString()
            };
            MainVM.MainWindow.TablesPage.tvTables.Items.Add(item);
        }
        MainVM.MainWindow.TablesPage.tvTables.IsEnabled = true;
        MainVM.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible;
    }

    public void GetAllDataFromTable(string tableName)
    {
        string sqlRequest = $"SELECT * FROM {tableName}";
        MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
    }

    public void GetColumnsOfTable(string tableName)
    {
        string sqlRequest = $"PRAGMA table_info({tableName});";
        MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
    }

    public void GetForeignKeys(string tableName)
    {
        string sqlRequest = $"PRAGMA foreign_key_list('{tableName}');";
        MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
    }

    public void GetTriggers(string tableName)
    {
        string sqlRequest = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
        MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
    }

    public void GetSqlDefinition(string tableName)
    {
        string sqlRequest = string.Format(@"SELECT sql FROM sqlite_master WHERE type='table' AND name LIKE '{0}'", tableName);
        DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = row["sql"].ToString();
        }
        else
        {
            MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
        }
    }

    public void SendSqlRequest()
    {
        if (UserDbConnection == null)
            throw new Exception("Database is not opened.");

        ResultCollection = UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
        MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = ResultCollection.DefaultView;

        MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible;
        MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true;
    }

    public DataTable SendSqlRequest(string sql)
    {
        if (AppDbConnection == null)
            throw new Exception("System RDBMS is not assigned.");
        return AppDbConnection.ExecuteSqlCommand(sql);
    }

    public void ClearTempTable(string tableName)
    {
        string sqlRequest = $"DELETE FROM {tableName};";
        AppDbConnection.ExecuteSqlCommand(sqlRequest);
    }
}
