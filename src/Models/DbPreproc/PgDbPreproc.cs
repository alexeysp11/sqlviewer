using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewerDatabase.DbConnections; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    public class PgDbPreproc : BaseRdbmsPreproc, IDbPreproc
    {
        private MainVM MainVM { get; set; }

        public PgDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            base.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public void CreateDb()
        {
            System.Windows.MessageBox.Show("Postgres CreateDb"); 
        }
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("Postgres OpenDb"); 
        } 

        public void InitUserDbConnection()
        {
            try
            {
                if (RepoHelper.AppSettingsRepo == null)
                    throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
                if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.PostgreSQL)
                    throw new System.Exception($"Unable to initialize base.UserDbConnection, incorrect ActiveRdbms: '{RepoHelper.AppSettingsRepo.ActiveRdbms}'.");
                
                if (RepoHelper.AppSettingsRepo != null)
                    base.UserDbConnection = new PgDbConnection();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void DisplayTablesInDb()
        {
            if (base.UserDbConnection == null)
            {
                return; 
            }

            try
            {
                string sqlRequest = MainVM.DataVM.MainDbBranch.GetSqlRequest("Postgres\\TableInfo\\DisplayTablesInDb.sql"); 
                DataTable dt = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
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
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetColumnsOfTable(string tableName)
        {
            string[] tn = tableName.Split('.');
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.GetSqlRequest("Postgres\\TableInfo\\GetColumns.sql"), tn[0], tn[1]); 
                MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.GetSqlRequest("Postgres\\TableInfo\\GetForeignKeys.sql"), tableName);
                MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.GetSqlRequest("Postgres\\TableInfo\\GetTriggers.sql"), tableName);
                MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string[] tn = tableName.Split('.');
                string sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.GetSqlRequest("Postgres\\TableInfo\\GetSqlDefinition.sql"), tn[0], tn[1]);
                DataTable dt = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
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
                throw ex; 
            }
        } 

        public void SendSqlRequest()
        {
            try
            {
                if (base.UserDbConnection == null)
                    throw new System.Exception("Database is not opened."); 

                DataTable resultCollection = base.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
                MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = resultCollection.DefaultView;

                MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
                MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public DataTable SendSqlRequest(string sql)
        {
            try
            {
                if (base.AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                return base.AppDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sql);
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void ClearTempTable(string tableName)
        {

        } 

        public ICommonDbConnectionSV GetAppDbConnection()
        {
            return base.AppDbConnection; 
        }
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return base.UserDbConnection; 
        }

        private string GetConnString()
        {
            return System.String.Format("Server={0};Username={1};Database={2};Port={3};Password={4}", 
                RepoHelper.AppSettingsRepo.DbHost,
                RepoHelper.AppSettingsRepo.DbUsername,
                RepoHelper.AppSettingsRepo.DbName,
                RepoHelper.AppSettingsRepo.DbPort,
                RepoHelper.AppSettingsRepo.DbPassword); 
        }
    }
}
