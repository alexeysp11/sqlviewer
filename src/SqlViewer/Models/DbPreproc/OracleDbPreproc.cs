using System.Data;
using System.Windows; 
using System.Windows.Controls; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewer.Models.DbConnections; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    public class OracleDbPreproc : IDbPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public OracleDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public void CreateDb()
        {
            System.Windows.MessageBox.Show("Oracle CreateDb"); 
        }
        public void OpenDb()
        {
            System.Windows.MessageBox.Show("Oracle OpenDb"); 
        } 

        public void InitUserDbConnection()
        {
            if (RepoHelper.AppSettingsRepo == null)
                throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
            if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.Oracle)
                throw new System.Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: '{RepoHelper.AppSettingsRepo.ActiveRdbms}'.");
                
            if (RepoHelper.AppSettingsRepo != null)
                UserDbConnection = new OracleDbConnection();
        }

        public void DisplayTablesInDb()
        {
            if (UserDbConnection == null)
            {
                return; 
            }

            string sqlRequest = "SELECT ut.table_name AS name FROM user_tables ut";
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

        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
            }
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 
        public void GetColumnsOfTable(string tableName)
        {
            string[] tn = tableName.Split('.');
            string sqlRequest = string.Format(@"
SELECT column_name, data_type, data_length
FROM USER_TAB_COLUMNS
WHERE UPPER(table_name) = UPPER('{0}')", tn[0], tn[1]); 
            MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        } 
        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(@"
SELECT * FROM all_constraints WHERE r_constraint_name IN
(
    SELECT constraint_name
    FROM all_constraints
    WHERE UPPER(table_name) LIKE UPPER('{0}')
)", tableName);
                MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
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
                string sqlRequest = string.Format(@"SELECT * FROM all_triggers WHERE UPPER(table_name) LIKE UPPER('{0}')", tableName);
                MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
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
                string sqlRequest = string.Format(@"
select dbms_metadata.get_ddl('TABLE', table_name) as sql
from user_tables ut
WHERE UPPER(ut.table_name) LIKE UPPER('{0}')", tableName);
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
            catch (System.Exception ex)
            {
                throw ex; 
            }
        } 

        public void SendSqlRequest()
        {
            try
            {
                if (UserDbConnection == null)
                    throw new System.Exception("Database is not opened."); 

                DataTable resultCollection = UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
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
                if (AppDbConnection == null)
                    throw new System.Exception("System RDBMS is not assigned."); 
                return AppDbConnection.ExecuteSqlCommand(sql);
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
            return AppDbConnection; 
        }
        public ICommonDbConnectionSV GetUserDbConnection()
        {
            return UserDbConnection; 
        }
    }
}
