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
    /// <summary>
    /// 
    /// </summary>
    public class DatabasePreproc
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        public DatabasePreproc(MainVM mainVM)
        {
            this.MainVM = mainVM; 
            //MainVM.DataVM.MainDbBranch.DbConnectionPreproc = MainVM.DataVM.MainDbBranch.MainVM.DataVM.MainDbBranch.DbConnectionPreproc; 
            // MainVM.DataVM.MainDbBranch.DbConnectionPreproc.AppDbConnection = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db"); 
        }

        /// <summary>
        /// Creates database using UserRdbmsPreproc
        /// </summary>
        public void CreateDb()
        {
            try
            {
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        SqlViewer.Helpers.FileSysHelper.SaveLocalFile(); 
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        System.Windows.MessageBox.Show("PostgreSQL CreateDb"); 
                        break;

                    case RdbmsEnum.MySQL: 
                        System.Windows.MessageBox.Show("MySQL CreateDb"); 
                        break;

                    case RdbmsEnum.Oracle: 
                        System.Windows.MessageBox.Show("Oracle CreateDb"); 
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Opens database using UserRdbmsPreproc
        /// </summary>
        public void OpenDb()
        {
            try
            {
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        string path = SqlViewer.Helpers.FileSysHelper.OpenLocalFile(); 
                        if (path == string.Empty) return; 
                        MainVM.DataVM.MainDbBranch.DbConnectionPreproc.InitLocalDbConnection(path); 
                        DisplayTablesInDb();
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        System.Windows.MessageBox.Show("PostgreSQL OpenDb"); 
                        break;

                    case RdbmsEnum.MySQL: 
                        System.Windows.MessageBox.Show("MySQL OpenDb"); 
                        break;

                    case RdbmsEnum.Oracle: 
                        System.Windows.MessageBox.Show("Oracle OpenDb"); 
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Displays all tables that a database contains 
        /// </summary>
        public void DisplayTablesInDb()
        {
            if (MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection == null)
                return; 
            try
            {
                string sqlRequest; 
                DataTable dt; 
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite\\TableInfo\\DisplayTablesInDb.sql"); 
                        dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Postgres\\TableInfo\\DisplayTablesInDb.sql"); 
                        dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
                        break;

                    case RdbmsEnum.MySQL: 
                        sqlRequest = string.Format(MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Mysql\\TableInfo\\DisplayTablesInDb.sql"), RepoHelper.AppSettingsRepo.DatabaseSettings.DbName); 
                        dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
                        break;

                    case RdbmsEnum.Oracle: 
                        sqlRequest = MainVM.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Oracle\\TableInfo\\DisplayTablesInDb.sql"); 
                        dt = MainVM.DataVM.MainDbBranch.DbConnectionPreproc.UserDbConnection.SetConnString(GetConnString()).ExecuteSqlCommand(sqlRequest);
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
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
                System.Windows.MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private string GetConnString()
        {
            switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
            {
                case RdbmsEnum.PostgreSQL: 
                    return System.String.Format("Server={0};Username={1};Database={2};Port={3};Password={4}", 
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbName,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword); 

                case RdbmsEnum.MySQL: 
                    return string.Format("Server={0}; database={1}; UID={2}; password={3}",  
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbName,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername,
                        //RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort,
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword);

                case RdbmsEnum.Oracle: 
                    return $@"Data Source=(DESCRIPTION =
    (ADDRESS_LIST =
      (ADDRESS = (PROTOCOL = TCP)(HOST = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost})(PORT = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort}))
    )
    (CONNECT_DATA =
      (SERVICE_NAME = {RepoHelper.AppSettingsRepo.DatabaseSettings.DbName})
    )
  ); User ID={RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername};Password={RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword};"; 

                default:
                    throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
            }
        }
    }
}
