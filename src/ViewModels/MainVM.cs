using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Documents; 
using System.Windows.Input; 
using SqlViewer.Commands; 
using SqlViewer.Models; 
using SqlViewer.Utils.Language; 
using SqlViewer.Helpers; 

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public MainWindow MainWindow { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public DataVM DataVM { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public VisualVM VisualVM { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public ICommand DbCommand { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public ICommand HelpCommand { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public ICommand RedirectCommand { get; private set; } 
        /// <summary>
        /// 
        /// </summary>
        public ICommand AppCommand { get; private set; } 

        /// <summary>
        /// 
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
            
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 
            this.RedirectCommand = new RedirectCommand(this); 
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
                
                var appSettingsRepo = new SqlViewer.Models.DataStorage.AppSettingsRepo(); 
                
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
        /// 
        /// </summary>
        public void RecoverSettings()
        {
            string msg = "Are you sure to recover settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.DataVM.MainDbBranch.ClearTempTable("tmp_settings");
                
                string sql = this.DataVM.MainDbBranch.GetSqlRequest("Sqlite/App/RecoverSettings.sql"); 
                this.DataVM.MainDbBranch.SendSqlRequest(sql); 
                InitAppRepository(); 
                Translate(); 
                this.VisualVM.InitUI(); 

                System.Windows.MessageBox.Show("Settings recovered", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveSettings()
        {
            string msg = "Are you sure to save settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Save settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try 
                {
                    this.DataVM.MainDbBranch.ClearTempTable("tmp_settings"); 
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
        /// 
        /// </summary>
        public void CancelSettings()
        {
            string msg = "Are you sure to cancel settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Cancel settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.DataVM.MainDbBranch.ClearTempTable("tmp_settings"); 
                
                ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).CancelChangesAppRepository(); 
                
                System.Windows.MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                this.VisualVM.SettingsView.Close(); 
            }
        }
        #endregion  // Settings methods 

        /// <summary>
        /// 
        /// </summary>
        public void ExitApplication()
        {
            string msg = "Are you sure to close the application?"; 
            if (System.Windows.MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
                //SqlViewer.Helpers.FileSysHelper.ExecuteCmd("taskkill /f /im SqlViewer.exe /t"); 
            }
        }
    }
}
