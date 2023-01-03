using System.Data; 
using System.Windows; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewer.Models.DbConnections; 
using ICommonDbConnectionSV = SqlViewer.Models.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    public class PgDbPreproc : IRdbmsPreproc
    {
        private MainVM MainVM { get; set; }

        public ICommonDbConnectionSV AppDbConnection { get; private set; }
        public ICommonDbConnectionSV UserDbConnection { get; private set; }

        public PgDbPreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            this.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        public void CreateDb()
        {

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
                    throw new System.Exception($"Unable to initialize UserDbConnection, incorrect ActiveRdbms: '{RepoHelper.AppSettingsRepo.ActiveRdbms}'.");
                
                if (RepoHelper.AppSettingsRepo != null)
                    UserDbConnection = new PgDbConnection();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public void DisplayTablesInDb()
        {

        } 
        public void GetAllDataFromTable(string tableName)
        {

        } 
        public void GetColumnsOfTable(string tableName)
        {

        } 
        public void GetForeignKeys(string tableName)
        {

        } 
        public void GetTriggers(string tableName)
        {

        } 
        public void GetSqlDefinition(string tableName)
        {

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
