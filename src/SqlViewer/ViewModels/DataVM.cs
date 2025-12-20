using System.Data;
using System.Windows;
using SqlViewer.Models.DbPreproc;
using SqlViewer.Models.DbTransfer;
using SqlViewer.Helpers;
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms;
using System.IO;

namespace SqlViewer.ViewModels;

public class DataVM
{
    private MainVM MainVM { get; set; }

    public IDbPreproc AppRdbmsPreproc { get; private set; }
    public IDbPreproc UserRdbmsPreproc { get; private set; }

    public DbInterconnection DbInterconnection { get; private set; }

    public DataVM(MainVM mainVM)
    {
        MainVM = mainVM;
        AppRdbmsPreproc = new SqliteDbPreproc(MainVM);
        DbInterconnection = new DbInterconnection();
    }

    #region Primary DB operations 
    public void CreateDb()
    {
        try
        {
            UserRdbmsPreproc.CreateDb();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Opens DB 
    /// </summary>
    public void OpenDb()
    {
        try
        {
            InitUserDbConnection();
            UserRdbmsPreproc.OpenDb();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public static string GetSqlRequest(string filename)
    {
        return File.ReadAllText($"{SettingsHelper.RootFolder}\\Queries\\{filename}");
    }
    #endregion  // Primary DB operations 

    #region Initialization 
    public void InitUserDbConnection()
    {
        try
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
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion  // Initialization 

    #region // DB information 
    public void DisplayTablesInDb()
    {
        try
        {
            UserRdbmsPreproc.DisplayTablesInDb();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void GetAllDataFromTable(string tableName)
    {
        try
        {
            UserRdbmsPreproc.GetAllDataFromTable(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void GetColumnsOfTable(string tableName)
    {
        try
        {
            UserRdbmsPreproc.GetColumnsOfTable(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void GetForeignKeys(string tableName)
    {
        try
        {
            UserRdbmsPreproc.GetForeignKeys(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void GetTriggers(string tableName)
    {
        try
        {
            UserRdbmsPreproc.GetTriggers(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void GetSqlDefinition(string tableName)
    {
        try
        {
            UserRdbmsPreproc.GetSqlDefinition(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion  // DB information 

    #region Low-level operations
    public void SendSqlRequest()
    {
        try
        {
            UserRdbmsPreproc.SendSqlRequest();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return dt;
    }

    public void ClearTempTable(string tableName)
    {
        try
        {
            AppRdbmsPreproc.ClearTempTable(tableName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion  // Low-level operations 
}
