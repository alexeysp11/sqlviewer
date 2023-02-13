using System.Windows; 
using SqlViewer.Helpers; 
using SqlViewer.ViewModels; 
using SqlViewer.Views; 
using SqlViewerDatabase.DbConnections; 
using UserControlsMenu = SqlViewer.UserControls.Menu; 
using ICommonDbConnectionSV = SqlViewerDatabase.DbConnections.ICommonDbConnection; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.Models.DbPreproc
{
    /// <summary>
    /// 
    /// </summary>
    public class DbConnectionPreproc 
    {
        /// <summary>
        /// Main ViewModel
        /// </summary>
        private MainVM MainVM { get; set; }

        public DbConnectionPreproc(MainVM mainVM) => MainVM = mainVM;
        
        /// <summary>
        /// Database connection instance on the App layer  
        /// </summary>
        public ICommonDbConnectionSV AppDbConnection { get; private set; } = new SqliteDbConnection($"{SettingsHelper.GetRootFolder()}\\data\\app.db");
        /// <summary>
        /// Database connection instance on the User layer  
        /// </summary>
        public ICommonDbConnectionSV UserDbConnection { get; set; }

        /// <summary>
        /// initializes UserRdbmsPreproc
        /// </summary>
        public void InitUserDbConnection()
        {
            try
            {
                if (RepoHelper.AppSettingsRepo == null)
                    throw new System.Exception("RepoHelper.AppSettingsRepo is not assigned."); 
                switch (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms)
                {
                    case RdbmsEnum.SQLite: 
                        if (!string.IsNullOrEmpty(RepoHelper.AppSettingsRepo.DatabaseSettings.DbName))
                            UserDbConnection = new SqliteDbConnection(RepoHelper.AppSettingsRepo.DatabaseSettings.DbName);
                        break; 

                    case RdbmsEnum.PostgreSQL: 
                        UserDbConnection = new PgDbConnection();
                        break;

                    case RdbmsEnum.MySQL: 
                        UserDbConnection = new MysqlDbConnection();
                        break;

                    case RdbmsEnum.Oracle: 
                        UserDbConnection = new OracleDbConnection();
                        break;

                    default:
                        throw new System.Exception($"Unable to call RDBMS preprocessing unit, incorrect ActiveRdbms: {RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms}.");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Initializes DbConnection after OpenFileDialog (could only be used when SQLite is selected as an active RDBMS)
        /// </summary>
        public void InitLocalDbConnection(string path)
        {
            try
            {
                if (RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms != RdbmsEnum.SQLite)
                    throw new System.Exception("In order to initialize local database connection SQLite should be selected as an active RDBMS"); 

                UserDbConnection = new SqliteDbConnection(path);
                MainVM.MainWindow.SqlPage.tblDbName.Text = path;
                MainVM.MainWindow.TablesPage.tblDbName.Text = path;

                if (MainVM.VisualVM.SettingsView != null)
                    ((SettingsView)(MainVM.VisualVM.SettingsView)).tbDatabase.Text = path;
                if (MainVM.VisualVM.LoginView != null)
                    ((LoginView)(MainVM.VisualVM.LoginView)).tbDatabase.Text = path; 

                RepoHelper.AppSettingsRepo.DatabaseSettings.SetDbName(path); 
                
                // ??? 
                if (this.MainVM.VisualVM.Menu != null)
                    ((UserControlsMenu)this.MainVM.VisualVM.Menu).Init(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }
    }
}
