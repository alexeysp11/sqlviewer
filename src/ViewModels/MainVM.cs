using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Documents; 
using System.Windows.Input; 
using SqlViewer.Commands; 
using SqlViewer.Models; 
using SqlViewer.Models.DataStorage; 
using SqlViewer.Utils.Language; 
using SqlViewer.Helpers; 
using Microsoft.Extensions.Configuration; 
using CommandEnum = SqlViewer.Enums.Common.Command; 
using AppCommandEnum = SqlViewer.Enums.Common.AppCommand; 
using DbCommandEnum = SqlViewer.Enums.Common.DbCommand; 
using RedirectCommandEnum = SqlViewer.Enums.Common.RedirectCommand; 

namespace SqlViewer.ViewModels
{
    /// <summary>
    /// Main ViewModel that is stores references to other modules (ViewModels, Commands, Translator etc) 
    /// and performs operations that are exteremely important for the application 
    /// </summary>
    public class MainVM
    {
        #region Properties
        /// <summary>
        /// Main window of the application 
        /// </summary>
        public MainWindow MainWindow { get; private set; } 

        /// <summary>
        /// ViewModel for performing operations with data sources (e.g. databases) 
        /// </summary>
        public DataVM DataVM { get; private set; } 
        /// <summary>
        /// ViewModels for visual elements  
        /// </summary>
        public VisualVM VisualVM { get; private set; } 

        /// <summary>
        /// Main command of the application 
        /// </summary>
        public ICommand AppCommand { get; private set; } 

        /// <summary>
        /// Instance that performs translation for visual elements of the application 
        /// </summary>
        public Translator Translator { get; private set; }  
        #endregion  // Properties

        /// <summary>
        /// 
        /// </summary>
        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.DataVM = new DataVM(this); 
            this.VisualVM = new VisualVM(this); 
            
            this.AppCommand = new AppCommand(this); 

            (this.Translator = new Translator(this)).SetAppDbConnection((SqlViewerDatabase.DbConnections.SqliteDbConnection)this.DataVM.MainDbBranch.AppRdbmsPreproc.GetAppDbConnection()); 
        }

        #region Initialization 
        /// <summary>
        /// Initializes AppRepository and UserDbConnection after getting settings from DB 
        /// </summary>
        public void InitAppRepository()
        {
            try
            {
                DataTable dt = this.DataVM.MainDbBranch.SendSqlRequest(this.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/SelectFromSettings.sql")); 
                
                var appSettingsRepo = new AppSettingsRepo(); 

                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
                appSettingsRepo.SetConfigSettings(config.GetSection("Settings").Get<ConfigSettings>()); 

                appSettingsRepo.SetLanguage(dt.Rows[0]["language"].ToString()); 
                appSettingsRepo.SetAutoSave(dt.Rows[0]["auto_save"].ToString()); 
                appSettingsRepo.SetFontSize(System.Convert.ToInt32(dt.Rows[0]["font_size"])); 
                appSettingsRepo.SetFontFamily(dt.Rows[0]["font_family"].ToString()); 
                appSettingsRepo.SetTabSize(System.Convert.ToInt32(dt.Rows[0]["tab_size"])); 
                appSettingsRepo.SetWordWrap(dt.Rows[0]["word_wrap"].ToString()); 
                appSettingsRepo.SetDefaultRdbms(dt.Rows[0]["default_rdbms"].ToString()); 
                appSettingsRepo.SetActiveRdbms(dt.Rows[0]["active_rdbms"].ToString()); 
                appSettingsRepo.SetDbHost(dt.Rows[0]["server"].ToString()); 
                appSettingsRepo.SetDbName(dt.Rows[0]["db_name"].ToString()); 
                appSettingsRepo.SetDbPort(dt.Rows[0]["port"].ToString()); 
                appSettingsRepo.SetDbSchema(dt.Rows[0]["schema_name"].ToString()); 
                appSettingsRepo.SetDbUsername(dt.Rows[0]["db_username"].ToString()); 
                appSettingsRepo.SetDbPassword(dt.Rows[0]["db_pswd"].ToString()); 

                RepoHelper.SetAppSettingsRepo(appSettingsRepo); 
                this.DataVM.MainDbBranch.InitUserDbConnection(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Translates all pages in the application 
        /// </summary>
        public void Translate()
        {
            try
            {
                this.Translator.SetLanguageEnum(RepoHelper.AppSettingsRepo.Language); 
                this.Translator.TranslateLogin();
                this.Translator.TranslateMenu(); 
                this.Translator.TranslateSettings(); 
                this.Translator.TranslatePages(); 
                this.Translator.TranslateConnection(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // Initialization 

        #region Settings methods
        /// <summary>
        /// Recovers app settings to default 
        /// </summary>
        public void RecoverSettings()
        {
            string msg = "Are you sure to recover settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string sql = this.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/RecoverSettings.sql"); 
                this.DataVM.MainDbBranch.SendSqlRequest(sql); 
                InitAppRepository(); 
                Translate(); 
                this.VisualVM.InitUI(); 

                System.Windows.MessageBox.Show("Settings recovered", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }

        /// <summary>
        /// Save new values entered on Settings view 
        /// </summary>
        public void SaveSettings()
        {
            string msg = "Are you sure to save settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Save settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try 
                {
                    ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).UpdateAppRepository(); 

                    string sql = this.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/UpdateSettingsEditor.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.Language, RepoHelper.AppSettingsRepo.AutoSave, 
                        RepoHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.FontSize), RepoHelper.AppSettingsRepo.FontFamily, 
                        RepoHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.TabSize), RepoHelper.AppSettingsRepo.WordWrap); 
                    this.DataVM.MainDbBranch.SendSqlRequest(sql); 

                    sql = this.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/UpdateSettingsDb.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.DefaultRdbms, RepoHelper.AppSettingsRepo.ActiveRdbms, 
                        RepoHelper.AppSettingsRepo.DbHost, RepoHelper.AppSettingsRepo.DbName, RepoHelper.AppSettingsRepo.DbPort, 
                        RepoHelper.AppSettingsRepo.DbSchema, RepoHelper.AppSettingsRepo.DbUsername, RepoHelper.AppSettingsRepo.DbPassword); 
                    this.DataVM.MainDbBranch.SendSqlRequest(sql); 

                    InitAppRepository(); 
                    Translate(); 
                    this.VisualVM.InitUI(); 
                    System.Windows.MessageBox.Show("Settings saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                    this.VisualVM.SettingsView.Close(); 
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Cancels all changes that has been made on the Settings view 
        /// </summary>
        public void CancelSettings()
        {
            string msg = "Are you sure to cancel settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Cancel settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).CancelChangesAppRepository(); 
                
                System.Windows.MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                this.VisualVM.SettingsView.Close(); 
            }
        }
        #endregion  // Settings methods 

        #region Common methods 
        /// <summary>
        /// Preprocess input parameter of AppCommand 
        /// </summary>
        private void PreprocAppCommand(string parameter)
        {
            switch (parameter)
            {
                case nameof(AppCommandEnum.ExitApplication):
                    ExitApplication(); 
                    break;
                    
                case nameof(AppCommandEnum.RecoverSettings):
                    RecoverSettings(); 
                    break;
                    
                case nameof(AppCommandEnum.SaveSettings):
                    SaveSettings(); 
                    break;
                    
                case nameof(AppCommandEnum.CancelSettings):
                    CancelSettings(); 
                    break;
                    
                default: 
                    System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameter}' inside AppCommand", "Exception"); 
                    break; 
            }
        }

        /// <summary>
        /// Preprocess input parameter of DbCommand 
        /// </summary>
        private void PreprocDbCommand(string parameter)
        {
            switch (parameter)
            {
                case nameof(DbCommandEnum.SendSql):
                    this.DataVM.MainDbBranch.SendSqlRequest(); 
                    break;
                    
                case nameof(DbCommandEnum.New):
                    this.DataVM.MainDbBranch.CreateDb(); 
                    break;
                    
                case nameof(DbCommandEnum.Open):
                    this.DataVM.MainDbBranch.OpenDb(); 
                    break;

                default: 
                    System.Windows.MessageBox.Show($"Incorrect CommandParameter: '{parameter}' inside DbCommand", "Exception"); 
                    break; 
            }
        }

        /// <summary>
        /// Preprocess input parameter of RedirectCommand 
        /// </summary>
        private void PreprocRedirectCommand(string parameter)
        {
            switch (parameter)
            {
                case nameof(RedirectCommandEnum.SqlQuery):
                    this.VisualVM.RedirectToSqlQuery(); 
                    break;

                case nameof(RedirectCommandEnum.Tables):
                    this.VisualVM.RedirectToTables(); 
                    break;

                case nameof(RedirectCommandEnum.Settings):
                    this.VisualVM.OpenView("SettingsView"); 
                    break;

                case nameof(RedirectCommandEnum.Connections):
                    this.VisualVM.OpenView("ConnectionsView"); 
                    break;
                
                case nameof(RedirectCommandEnum.About):
                    this.VisualVM.OpenDocsInBrowser("About page", "User guide", $"{SqlViewer.Helpers.SettingsHelper.GetRootFolder()}\\docs\\About.html"); 
                    break;

                case nameof(RedirectCommandEnum.SqliteDocs):
                    this.VisualVM.OpenDocsInBrowser("SQLite documentation", "Common SQL docs", "https://www.sqlite.org/index.html");
                    break;

                case nameof(RedirectCommandEnum.PosgresDocs):
                    this.VisualVM.OpenDocsInBrowser("PosgreSQL documentation", "Common SQL docs", "https://www.postgresql.org/");
                    break;

                case nameof(RedirectCommandEnum.MySqlDocs):
                    this.VisualVM.OpenDocsInBrowser("MySQL documentation", "Common SQL docs", "https://dev.mysql.com/doc/"); 
                    break;

                case nameof(RedirectCommandEnum.OracleDocs):
                    this.VisualVM.OpenDocsInBrowser("Oracle documentation", "Common SQL docs", "https://docs.oracle.com/en/database/oracle/oracle-database/index.html"); 
                    break;

                default:
                    System.Windows.MessageBox.Show($"Incorrect parameter: '{parameter}' in RedirectCommand", "Error"); 
                    break;
            }
        }

        /// <summary>
        /// Performs preprocessing of the parameter that was sent to the AppCommand 
        /// </summary>
        public void PreprocCommandParameter(string parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(parameter))
                    throw new System.Exception("Parameter could not be null or empty"); 
                string[] subs = parameter.Split('.'); 
                if (subs.Length != 2)
                    throw new System.Exception("Incorrect number of arguments inside a paramter string"); 
                switch (subs[0])
                {
                    case nameof(CommandEnum.AppCommand): 
                        PreprocAppCommand(subs[1]); 
                        break; 
                    
                    case nameof(CommandEnum.DbCommand): 
                        PreprocDbCommand(subs[1]); 
                        break; 
                    
                    case nameof(CommandEnum.RedirectCommand): 
                        PreprocRedirectCommand(subs[1]); 
                        break; 
                    
                    default: 
                        throw new System.Exception("Incorrect name of Command");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception occured while trying to preprocess parameter string of Command: '{ex.Message}'", "Exception");
            }
        }

        /// <summary>
        /// Terminates the application 
        /// </summary>
        public void ExitApplication()
        {
            string msg = "Are you sure to close the application?"; 
            if (System.Windows.MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        #endregion  // Common methods 
    }
}
