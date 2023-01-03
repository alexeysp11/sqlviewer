using System.Collections.Generic; 
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

namespace SqlViewer.Models.DbPreproc 
{
    public class SqliteDbPreproc : IRdbmsPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public DataTable ResultCollection { get; private set; }
        public List<string> TablesCollection { get; private set; }

        public SqliteDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public ICommonDbConnectionSV GetAppDbConnection()
        {
            return AppDbConnection; 
        }
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return UserDbConnection; 
        }

        #region Primary DB operations
        /// <summary>
        /// Creates a local DB 
        /// </summary>
        public void CreateDb()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = SettingsHelper.GetFilterFileSystemDb(); 
                if (sfd.ShowDialog() == true)
                    System.IO.File.WriteAllText(sfd.FileName, string.Empty);
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        /// <summary>
        /// Opens a local DB using OpenFileDialog 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog(); 
                ofd.Filter = SettingsHelper.GetFilterFileSystemDb();
                if (ofd.ShowDialog() == true) {}

                string path = ofd.FileName; 
                if (path == string.Empty) return; 
                InitLocalDbConnection(path); 
                DisplayTablesInDb();
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }
        #endregion  // Primary DB operations

        #region DbConnection initialization
        public void InitUserDbConnection()
        {
            try
            {
                if (RepoHelper.AppSettingsRepo == null)
                    throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
                if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.SQLite)
                    throw new System.Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.ActiveRdbms}.");
                
                if (RepoHelper.AppSettingsRepo != null && !string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DbName))
                    UserDbConnection = new SqliteDbConnection(RepoHelper.AppSettingsRepo.DbName);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                // Maybe, this piece of code should be in VisualVM?  
                /*
                if (MainVM.MainWindow.TablesPage != null)
                {
                    var emptyDt = new DataTable(); 
                    //MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
                    //MainVM.MainWindow.TablesPage.tblDbName.Text = string.Empty;
                    MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.tbTableName.Text = string.Empty; 
                    MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
                }
                */
            }
        }

        /// <summary>
        /// Initializes DbConnection after OpenFileDialog 
        /// </summary>
        private void InitLocalDbConnection(string path)
        {
            try
            {
                UserDbConnection = new SqliteDbConnection(path);
                MainVM.MainWindow.SqlPage.tblDbName.Text = path;
                MainVM.MainWindow.TablesPage.tblDbName.Text = path;

                if (MainVM.VisualVM.SettingsView != null)
                    ((SettingsView)(MainVM.VisualVM.SettingsView)).tbDatabase.Text = path;
                if (MainVM.VisualVM.LoginView != null)
                    ((LoginView)(MainVM.VisualVM.LoginView)).tbDatabase.Text = path; 

                RepoHelper.AppSettingsRepo.SetDbName(path); 
                
                if (this.MainVM.VisualVM.Menu != null)
                    ((UserControlsMenu)this.MainVM.VisualVM.Menu).Init(); 
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }
        #endregion  // DbConnection initialization

        #region DB information 
        public void DisplayTablesInDb()
        {
            if (UserDbConnection == null)
            {
                return; 
            }

            try
            {
                string sqlRequest = MainVM.DataVM.GetSqlRequest("TableInfo\\DisplayTablesInDb.sql"); 
                DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
                MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    TreeViewItem item = new TreeViewItem(); 
                    item.Header = row["name"].ToString();
                    MainVM.MainWindow.TablesPage.tvTables.Items.Add(item); 
                }
                MainVM.MainWindow.TablesPage.tvTables.IsEnabled = true; 
                MainVM.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible; 
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA table_info({tableName});"; 
                MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA foreign_key_list('{tableName}');";
                MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
                MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.GetSqlRequest("TableInfo\\GetSqlDefinition.sql"), tableName);
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
            catch (System.Exception e)
            {
                throw e; 
            }
        }
        #endregion  // DB information  

        #region Low-level operations
        public void SendSqlRequest()
        {
            try
            {
                if (UserDbConnection == null)
                    throw new System.Exception("Database is not opened."); 

                ResultCollection = UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
                MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = ResultCollection.DefaultView;

                MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
                MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public DataTable SendSqlRequest(string sql)
        {
            try
            {
                if (AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                return AppDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }

        public void ClearTempTable(string tableName)
        {
            string sqlRequest = $"DELETE FROM {tableName};";
            try
            {
                AppDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception e)
            {
                throw e; 
            }
        }
        #endregion  // Low-level operations 
    } 
}
