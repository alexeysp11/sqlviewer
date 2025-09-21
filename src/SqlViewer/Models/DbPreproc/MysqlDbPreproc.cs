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
    public class MysqlDbPreproc : IDbPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public MysqlDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public void CreateDb()
        {
            System.Windows.MessageBox.Show("Mysql CreateDb"); 
        }

        public void OpenDb()
        {
            System.Windows.MessageBox.Show("Mysql OpenDb"); 
        } 

        public void InitUserDbConnection()
        {
            if (RepoHelper.AppSettingsRepo == null)
                throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
            if (RepoHelper.AppSettingsRepo.ActiveRdbms != RdbmsEnum.MySQL)
                throw new System.Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: '{RepoHelper.AppSettingsRepo.ActiveRdbms}'.");
                
            if (RepoHelper.AppSettingsRepo != null)
                UserDbConnection = new MysqlDbConnection();
        }

        public void DisplayTablesInDb()
        {
            if (UserDbConnection == null)
            {
                return; 
            }

            string sqlRequest = string.Format("SELECT table_name AS name FROM information_schema.tables WHERE table_schema = '{0}';", RepoHelper.AppSettingsRepo.DbName); 
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
            string sqlRequest = $"SELECT * FROM {tableName}"; 
            MainVM.MainWindow.TablesPage.dgrAllData.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetColumnsOfTable(string tableName)
        {
            string sqlRequest = string.Format(@"
SELECT 
`COLUMN_NAME`,
`ORDINAL_POSITION`,
`COLUMN_DEFAULT`,
`IS_NULLABLE`,
`DATA_TYPE`,
`CHARACTER_MAXIMUM_LENGTH`,
`NUMERIC_PRECISION`,
`COLUMN_TYPE`,
`COLUMN_COMMENT`,
`GENERATION_EXPRESSION`
FROM `INFORMATION_SCHEMA`.`COLUMNS`
WHERE UPPER(`TABLE_SCHEMA`) LIKE UPPER('{0}')
AND UPPER(`TABLE_NAME`) LIKE UPPER('{1}')", RepoHelper.AppSettingsRepo.DbName, tableName);
            MainVM.MainWindow.TablesPage.dgrColumns.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetForeignKeys(string tableName)
        {
            string sqlRequest = string.Format(@"
SELECT
TABLE_NAME,COLUMN_NAME,
CONSTRAINT_NAME,
REFERENCED_TABLE_NAME,
REFERENCED_COLUMN_NAME
FROM
INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE
UPPER(REFERENCED_TABLE_SCHEMA) LIKE UPPER('{0}')
AND UPPER(REFERENCED_TABLE_NAME) LIKE UPPER('{1}');", RepoHelper.AppSettingsRepo.DbName, tableName);
            MainVM.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetTriggers(string tableName)
        {
            string sqlRequest = string.Format("SHOW TRIGGERS LIKE '{0}'", tableName);
            MainVM.MainWindow.TablesPage.dgrTriggers.ItemsSource = UserDbConnection.ExecuteSqlCommand(sqlRequest).DefaultView;
        }

        public void GetSqlDefinition(string tableName)
        {
            string sqlRequest = string.Format("SHOW CREATE TABLE {0}", tableName);
            DataTable dt = UserDbConnection.ExecuteSqlCommand(sqlRequest);
            if (dt.Rows.Count > 0) 
            {
                DataRow row = dt.Rows[0];
                MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = row["Create Table"].ToString();
            }
            else 
            {
                MainVM.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
            }
        } 

        public void SendSqlRequest()
        {
            if (UserDbConnection == null)
                throw new System.Exception("Database is not opened.");

            DataTable resultCollection = UserDbConnection.ExecuteSqlCommand(MainVM.MainWindow.SqlPage.mtbSqlRequest.Text);
            MainVM.MainWindow.SqlPage.dbgSqlResult.ItemsSource = resultCollection.DefaultView;

            MainVM.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible;
            MainVM.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true;
        }

        public DataTable SendSqlRequest(string sql)
        {
            if (AppDbConnection == null)
                throw new System.Exception("System RDBMS is not assigned.");

            return AppDbConnection.ExecuteSqlCommand(sql);
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
