using System.Data;
using System.Windows;
using System.Windows.Input;
using SqlViewer.Commands;
using SqlViewer.Utils.Language;
using SqlViewer.Helpers;

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        #region Properties
        public MainWindow MainWindow { get; private set; } 

        public DataVM DataVM { get; private set; } 
        public VisualVM VisualVM { get; private set; } 

        public ConfigHelper ConfigHelper { get; private set; } 

        public ICommand DbCommand { get; private set; } 
        public ICommand HelpCommand { get; private set; } 
        public ICommand RedirectCommand { get; private set; } 
        public ICommand AppCommand { get; private set; } 

        public Translator Translator { get; private set; }  
        #endregion  // Properties

        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.DataVM = new DataVM(this); 
            this.VisualVM = new VisualVM(this); 

            this.ConfigHelper = new ConfigHelper(this, SettingsHelper.GetRootFolder()); 
            
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 
            this.RedirectCommand = new RedirectCommand(this); 
            this.AppCommand = new AppCommand(this); 

            (this.Translator = new Translator(this)).SetAppDbConnection((SqlViewer.Models.DbConnections.SqliteDbConnection)this.DataVM.AppRdbmsPreproc.GetAppDbConnection()); 
        }

        #region Initialization 
        /// <summary>
        /// Initializes AppRepository and UserDbConnection after getting settings from DB 
        /// </summary>
        public void InitAppRepository()
        {
            try
            {
                DataTable dt = this.DataVM.SendSqlRequest(this.DataVM.GetSqlRequest("Sqlite/App/SelectFromSettings.sql")); 
                
                string language = dt.Rows[0]["language"].ToString();
                string autoSave = dt.Rows[0]["auto_save"].ToString();
                int fontSize = System.Convert.ToInt32(dt.Rows[0]["font_size"]);
                string fontFamily = dt.Rows[0]["font_family"].ToString();
                int tabSize = System.Convert.ToInt32(dt.Rows[0]["tab_size"]);
                string wordWrap = dt.Rows[0]["word_wrap"].ToString();
                string defaultRdbms = dt.Rows[0]["default_rdbms"].ToString();
                string activeRdbms = dt.Rows[0]["active_rdbms"].ToString();
                string server = dt.Rows[0]["server"].ToString();
                string dbName = dt.Rows[0]["db_name"].ToString();
                string port = dt.Rows[0]["port"].ToString();
                string schemaName = dt.Rows[0]["schema_name"].ToString();
                string dbUsername = dt.Rows[0]["db_username"].ToString();
                string dbPswd = dt.Rows[0]["db_pswd"].ToString();

                var enumEncoder = EnumCodecHelper.EnumEncoder; 
                RepoHelper.SetAppSettingsRepo(new SqlViewer.Models.DataStorage.AppSettingsRepo(enumEncoder, language, autoSave, 
                    fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms, server, 
                    dbName, port, schemaName, dbUsername, dbPswd)); 
                this.DataVM.InitUserDbConnection(); 
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
        public void RecoverSettings()
        {
            string msg = "Are you sure to recover settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.DataVM.ClearTempTable("settings");
                
                string sql = this.DataVM.GetSqlRequest("Sqlite/App/RecoverSettings.sql"); 
                this.DataVM.SendSqlRequest(sql); 
                InitAppRepository(); 
                Translate(); 
                this.VisualVM.InitUI(); 

                System.Windows.MessageBox.Show("Settings recovered", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }

        public void SaveSettings()
        {
            string msg = "Are you sure to save settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Save settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try 
                {
                    this.DataVM.ClearTempTable("settings"); 
                    ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).UpdateAppRepository(); 

                    string sql = this.DataVM.GetSqlRequest("Sqlite/App/UpdateSettingsEditor.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.Language, RepoHelper.AppSettingsRepo.AutoSave, 
                        EnumCodecHelper.EnumDecoder.GetFontSizeName(RepoHelper.AppSettingsRepo.FontSize), RepoHelper.AppSettingsRepo.FontFamily, 
                        EnumCodecHelper.EnumDecoder.GetTabSizeName(RepoHelper.AppSettingsRepo.TabSize), RepoHelper.AppSettingsRepo.WordWrap); 
                    this.DataVM.SendSqlRequest(sql);

                    sql = this.DataVM.GetSqlRequest("Sqlite/App/UpdateSettingsDb.sql"); 
                    sql = string.Format(sql, RepoHelper.AppSettingsRepo.DefaultRdbms, RepoHelper.AppSettingsRepo.ActiveRdbms, 
                        RepoHelper.AppSettingsRepo.DbHost, RepoHelper.AppSettingsRepo.DbName, RepoHelper.AppSettingsRepo.DbPort, 
                        RepoHelper.AppSettingsRepo.DbSchema, RepoHelper.AppSettingsRepo.DbUsername, RepoHelper.AppSettingsRepo.DbPassword); 
                    this.DataVM.SendSqlRequest(sql); 

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

        public void CancelSettings()
        {
            string msg = "Are you sure to cancel settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Cancel settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.DataVM.ClearTempTable("settings"); 
                
                ((SqlViewer.Views.SettingsView)this.VisualVM.SettingsView).CancelChangesAppRepository(); 
                
                System.Windows.MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                this.VisualVM.SettingsView.Close(); 
            }
        }
        #endregion  // Settings methods 

        public void ExitApplication()
        {
            string msg = "Are you sure to close the application?"; 
            if (System.Windows.MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
