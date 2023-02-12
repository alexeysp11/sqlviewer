using System.Data; 
using System.Windows; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class TablePreproc 
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        public TablePreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            // DbConnectionPreproc.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        /// <summary>
        /// Gets all data from a table 
        /// </summary>
        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the columns that a table contains 
        /// </summary>
        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                string[] tn; 
                string sqlRequest; 
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        sqlRequest = $"PRAGMA table_info({tableName});"; 
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        tn = tableName.Split('.');
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetColumns.sql"), tn[0], tn[1]); 
                        break;

                    case RdbmsEnum.MySQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetColumns.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName, tableName); 
                        break;

                    case RdbmsEnum.Oracle: 
                        tn = tableName.Split('.');
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetColumns.sql"), tn[0], tn[1]); 
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the foreign keys that a table contains 
        /// </summary>
        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest; 
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        sqlRequest = $"PRAGMA foreign_key_list('{tableName}');";
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetTriggers.sql"), tableName);
                        break;

                    case RdbmsEnum.MySQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetTriggers.sql"), tableName);
                        break;

                    case RdbmsEnum.Oracle: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetTriggers.sql"), tableName);
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets info about all the triggers that a table contains 
        /// </summary>
        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest;
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        sqlRequest = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetForeignKeys.sql"), tableName);
                        break;

                    case RdbmsEnum.MySQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetForeignKeys.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName, tableName);;
                        break;

                    case RdbmsEnum.Oracle: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetForeignKeys.sql"), tableName);
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        /// <summary>
        /// Gets SQL query for recreating a table  
        /// </summary>
        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest; 
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\GetSqlDefinition.sql"), tableName);
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        string[] tn = tableName.Split('.');
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\GetSqlDefinition.sql"), tn[0], tn[1]);
                        break;

                    case RdbmsEnum.MySQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\GetSqlDefinition.sql"), tableName);
                        break;

                    case RdbmsEnum.Oracle: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\GetSqlDefinition.sql"), tableName);
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
                if (string.IsNullOrEmpty(sqlRequest))
                    throw new System.Exception("SQL request could not be empty after reading a file"); 
                DataTable dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
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
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
        
    }
}
