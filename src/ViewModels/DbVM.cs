using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using Microsoft.Win32;
using SqlViewer.Models.DbConnections; 
using SqlViewer.Helpers; 
using SqlViewer.Views; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 
using UserControlsMenu = SqlViewer.UserControls.Menu; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.ViewModels
{
    public class DbVM
    {
        private MainVM MainVM { get; set; }

        public SqliteDbConnection AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public DataTable ResultCollection { get; private set; }
        public List<string> TablesCollection { get; private set; }

        public DbVM(MainVM mainVM)
        {
            this.MainVM = mainVM; 

            string rootFolder = SettingsHelper.GetRootFolder(); 
            this.AppDbConnection = new SqliteDbConnection($"{rootFolder}\\data\\app.db"); 
        }

        #region User DB methods 
        public void InitUserDbConnection()
        {
            try
            {
                if (RepoHelper.AppSettingsRepo == null)
                {
                    throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
                }

                switch (RepoHelper.AppSettingsRepo.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        // Unable to set an empty path for UserDbConnection, so it's better to assign UserDbConnection as null.  
                        UserDbConnection = null; 
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        UserDbConnection = new PgDbConnection(); 
                        break;

                    case RdbmsEnum.MySQL: 
                        UserDbConnection = new MysqlDbConnection(); 
                        break;

                    default:
                        throw new System.Exception($"Unable to assign UserDbConnection, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.ActiveRdbms}.");
                        break;
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            finally
            {
                if (MainVM.MainWindow.SqlPage != null)
                { 
                    MainVM.MainWindow.SqlPage.tblSqlPagePath.Text = string.Empty;
                }
                
                if (MainVM.MainWindow.TablesPage != null)
                {
                    var emptyDt = new DataTable(); 
                    MainVM.MainWindow.TablesPage.tvTables.Items.Clear();
                    MainVM.MainWindow.TablesPage.tblTablesPagePath.Text = string.Empty;
                    MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = emptyDt.DefaultView;
                    MainVM.MainWindow.TablesPage.tbTableName.Text = string.Empty; 
                    MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
                }
            }
        }

        private void InitLocalDbConnection(string path)
        {
            try
            {
                UserDbConnection = new SqliteDbConnection(path);
                MainVM.MainWindow.SqlPage.tblSqlPagePath.Text = path;
                MainVM.MainWindow.TablesPage.tblTablesPagePath.Text = path;

                RepoHelper.AppSettingsRepo.SetDbName(path); 
                
                ((UserControlsMenu)this.MainVM.VisualVM.Menu).Init(); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void DisplayTablesInDb()
        {
            if (UserDbConnection == null)
            {
                return; 
            }

            try
            {
                string sqlRequest = GetSqlRequest("TableInfo\\DisplayTablesInDb.sql"); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 

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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(GetSqlRequest("TableInfo\\GetSqlDefinition.sql"), tableName);
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
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public string GetSqlRequest(string filename)
        {
            string sqlRequest = string.Empty; 
            try
            {
                sqlRequest = System.IO.File.ReadAllText($"{SettingsHelper.GetRootFolder()}\\src\\Queries\\{filename}"); 
            }
            catch (System.Exception e) 
            {
                throw e; 
            }
            return sqlRequest; 
        }

        public void CreateLocalDb()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = SettingsHelper.GetFilterFileSystemDb(); 
                if (sfd.ShowDialog() == true)
                    System.IO.File.WriteAllText(sfd.FileName, string.Empty);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenLocalDb()
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
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // User DB methods 

        #region System DB methods
        public DataTable SendSqlRequest(string sql)
        {
            DataTable dt = new DataTable(); 
            try
            {
                if (AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                dt = AppDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            return dt; 
        }

        public void ClearTempTable(string tableName)
        {
            string sqlRequest = $"DELETE FROM {tableName};";
            try
            {
                AppDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // System DB methods       
    }
}
