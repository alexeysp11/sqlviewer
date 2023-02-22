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
            RepoHelper.LoggingHub.WriteLog("MainVM.MainVM: begin"); 

            this.MainWindow = mainWindow; 
            
            this.DataVM = new DataVM(this); 
            this.VisualVM = new VisualVM(this); 
            this.AppCommand = new AppCommand(this); 
            (this.Translator = new Translator(this)).SetAppDbConnection((SqlViewerDatabase.DbConnections.SqliteDbConnection)this.DataVM.MainDbBranch.GetAppDbConnection()); 

            RepoHelper.LoggingHub.WriteLog("MainVM.MainVM: finished"); 
        }

        #region Initialization 
        /// <summary>
        /// Initializes AppRepository and UserDbConnection after getting settings from DB 
        /// </summary>
        public void InitAppRepository()
        {
            try
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.InitAppRepository: begin"); 

                DataTable dt = this.DataVM.MainDbBranch.RequestPreproc.SendSqlRequest(this.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite/App/SelectFromSettings.sql")); 
                var appSettingsRepo = new AppSettingsRepo(); 
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
                appSettingsRepo.SetConfigSettings(config.GetSection("Settings").Get<ConfigSettings>()); 

                appSettingsRepo.SetLanguage(dt.Rows[0]["language"].ToString()); 
                appSettingsRepo.EditorSettings.SetAutoSave(dt.Rows[0]["auto_save"].ToString()); 
                appSettingsRepo.EditorSettings.SetFontSize(System.Convert.ToInt32(dt.Rows[0]["font_size"])); 
                appSettingsRepo.EditorSettings.SetFontFamily(dt.Rows[0]["font_family"].ToString()); 
                appSettingsRepo.EditorSettings.SetTabSize(System.Convert.ToInt32(dt.Rows[0]["tab_size"])); 
                appSettingsRepo.EditorSettings.SetWordWrap(dt.Rows[0]["word_wrap"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDefaultRdbms(dt.Rows[0]["default_rdbms"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetActiveRdbms(dt.Rows[0]["active_rdbms"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbHost(dt.Rows[0]["server"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbName(dt.Rows[0]["db_name"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbPort(dt.Rows[0]["port"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbSchema(dt.Rows[0]["schema_name"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbUsername(dt.Rows[0]["db_username"].ToString()); 
                appSettingsRepo.DatabaseSettings.SetDbPassword(dt.Rows[0]["db_pswd"].ToString()); 

                RepoHelper.SetAppSettingsRepo(appSettingsRepo); 
                this.DataVM.MainDbBranch.DbConnectionPreproc.InitUserDbConnection(); 

                RepoHelper.LoggingHub.WriteLog("MainVM.InitAppRepository: finish"); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                RepoHelper.LoggingHub.WriteLog($"MainVM.InitAppRepository: exception (msg: '{ex.Message}')"); 
            }
        }

        /// <summary>
        /// Translates all pages in the application 
        /// </summary>
        public void Translate()
        {
            try
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.Translate: begin"); 

                this.Translator.SetLanguageEnum(RepoHelper.AppSettingsRepo.Language); 
                //this.Translator.TranslateLanguage();
                this.Translator.TranslateLogin();
                this.Translator.TranslateMenu(); 
                this.Translator.TranslateSettings(); 
                this.Translator.TranslatePages(); 
                this.Translator.TranslateConnection(); 

                RepoHelper.LoggingHub.WriteLog("MainVM.Translate: finish"); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                RepoHelper.LoggingHub.WriteLog($"MainVM.Translate: exception (msg: '{ex.Message}')"); 
            }
        }
        #endregion  // Initialization 

        #region Settings methods
        /// <summary>
        /// Recovers app settings to default 
        /// </summary>
        public void RecoverSettings()
        {
            RepoHelper.LoggingHub.WriteLog("MainVM.RecoverSettings: begin"); 

            string msg = "Are you sure to recover settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.RecoverSettings: processing"); 

                string sql = this.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite/App/RecoverSettings.sql"); 
                this.DataVM.MainDbBranch.RequestPreproc.SendSqlRequest(sql); 
                InitAppRepository(); 
                Translate(); 
                this.VisualVM.InitUI(); 

                System.Windows.MessageBox.Show("Settings recovered", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                RepoHelper.LoggingHub.WriteLog("MainVM.RecoverSettings: processed"); 
            }
            RepoHelper.LoggingHub.WriteLog("MainVM.RecoverSettings: finish"); 
        }

        /// <summary>
        /// Save new values entered on Settings view 
        /// </summary>
        public void SaveSettings()
        {
            RepoHelper.LoggingHub.WriteLog("MainVM.SaveSettings: begin"); 

            string msg = "Are you sure to save settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Save settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try 
                {
                    RepoHelper.LoggingHub.WriteLog("MainVM.SaveSettings: processing"); 

                    ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).UpdateAppRepository(); 

                    string sql = this.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite/App/UpdateSettingsEditor.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.Language, RepoHelper.AppSettingsRepo.EditorSettings.AutoSave, 
                        RepoHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.EditorSettings.FontSize), RepoHelper.AppSettingsRepo.EditorSettings.FontFamily, 
                        RepoHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.EditorSettings.TabSize), RepoHelper.AppSettingsRepo.EditorSettings.WordWrap); 
                    this.DataVM.MainDbBranch.RequestPreproc.SendSqlRequest(sql); 

                    sql = this.DataVM.MainDbBranch.RequestPreproc.GetSqlRequestFromFile("Sqlite/App/UpdateSettingsDb.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.DatabaseSettings.DefaultRdbms, RepoHelper.AppSettingsRepo.DatabaseSettings.ActiveRdbms, 
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbHost, RepoHelper.AppSettingsRepo.DatabaseSettings.DbName, RepoHelper.AppSettingsRepo.DatabaseSettings.DbPort, 
                        RepoHelper.AppSettingsRepo.DatabaseSettings.DbSchema, RepoHelper.AppSettingsRepo.DatabaseSettings.DbUsername, RepoHelper.AppSettingsRepo.DatabaseSettings.DbPassword); 
                    this.DataVM.MainDbBranch.RequestPreproc.SendSqlRequest(sql); 

                    InitAppRepository(); 
                    Translate(); 
                    this.VisualVM.InitUI(); 
                    System.Windows.MessageBox.Show("Settings saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                    this.VisualVM.SettingsView.Close(); 

                    RepoHelper.LoggingHub.WriteLog("MainVM.SaveSettings: finish"); 
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    RepoHelper.LoggingHub.WriteLog($"MainVM.SaveSettings: exception (msg: '{ex.Message}')"); 
                }
            }
        }

        /// <summary>
        /// Cancels all changes that has been made on the Settings view 
        /// </summary>
        public void CancelSettings()
        {
            RepoHelper.LoggingHub.WriteLog("MainVM.CancelSettings: begin"); 

            string msg = "Are you sure to cancel settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Cancel settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.CancelSettings: processing"); 

                ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).CancelChangesAppRepository(); 
                System.Windows.MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                this.VisualVM.SettingsView.Close(); 

                RepoHelper.LoggingHub.WriteLog("MainVM.CancelSettings: processed"); 
            }
            RepoHelper.LoggingHub.WriteLog("MainVM.CancelSettings: finish"); 
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
            RepoHelper.LoggingHub.WriteLog("MainVM.PreprocDbCommand: begin"); 
            switch (parameter)
            {
                case nameof(DbCommandEnum.SendSql):
                    this.DataVM.MainDbBranch.RequestPreproc.SendSqlRequest(); 
                    break;
                    
                case nameof(DbCommandEnum.New):
                    this.DataVM.MainDbBranch.DatabasePreproc.CreateDb(); 
                    break;
                    
                case nameof(DbCommandEnum.Open):
                    this.DataVM.MainDbBranch.DatabasePreproc.OpenDb(); 
                    break;

                default: 
                    string errMsg = $"Incorrect parameter: '{parameter}'"; 
                    System.Windows.MessageBox.Show(errMsg, "Error"); 
                    RepoHelper.LoggingHub.WriteLog($"MainVM.PreprocDbCommand: error (msg: {errMsg})"); 
                    break; 
            }
            RepoHelper.LoggingHub.WriteLog("MainVM.PreprocDbCommand: end"); 
        }

        /// <summary>
        /// Preprocess input parameter of RedirectCommand 
        /// </summary>
        private void PreprocRedirectCommand(string parameter)
        {
            RepoHelper.LoggingHub.WriteLog($"MainVM.PreprocRedirectCommand: begin (parameter: '{parameter}')"); 
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
                    string errMsg = $"Incorrect parameter: '{parameter}'"; 
                    System.Windows.MessageBox.Show(errMsg, "Error"); 
                    RepoHelper.LoggingHub.WriteLog($"MainVM.PreprocRedirectCommand: error (msg: {errMsg})"); 
                    break;
            }
            RepoHelper.LoggingHub.WriteLog("MainVM.PreprocRedirectCommand: end"); 
        }

        /// <summary>
        /// Performs preprocessing of the parameter that was sent to the AppCommand 
        /// </summary>
        public void PreprocCommandParameter(string parameter)
        {
            try
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.PreprocCommandParameter: begin"); 

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
                RepoHelper.LoggingHub.WriteLog("MainVM.PreprocCommandParameter: finish"); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception occured while trying to preprocess parameter string of Command: '{ex.Message}'", "Exception");
                RepoHelper.LoggingHub.WriteLog($"MainVM.PreprocCommandParameter: exception (msg: '{ex.Message}')"); 
            }
        }

        /// <summary>
        /// Terminates the application 
        /// </summary>
        public void ExitApplication()
        {
            RepoHelper.LoggingHub.WriteLog("MainVM.ExitApplication: begin"); 

            string msg = "Are you sure to close the application?"; 
            if (System.Windows.MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepoHelper.LoggingHub.WriteLog("MainVM.ExitApplication: end"); 
                System.Windows.Application.Current.Shutdown();
            }
        }
        #endregion  // Common methods 
    }
}
