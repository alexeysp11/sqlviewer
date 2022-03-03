using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Documents; 
using System.Windows.Input; 
using Microsoft.Win32;
using SqlViewer.Commands; 
using SqlViewer.Models; 
using SqlViewer.Models.EnumOperations; 
using SqlViewer.Models.DbConnections; 
using SqlViewer.Views; 
using SqlViewer.Utils.Language; 
using IDbConnectionSqlViewer = SqlViewer.Models.DbConnections.IDbConnection; 
using UserControlsMenu = SqlViewer.UserControls.Menu; 
using RdbmsEnum = SqlViewer.Enums.Database.Rdbms; 

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        public MainWindow MainWindow { get; private set; } 

        public Config Config { get; private set; } 

        public AppRepository AppRepository { get; private set; } = null; 

        public SqliteDbConnection AppDbConnection { get; private set; }
        public IDbConnectionSqlViewer UserDbConnection { get; private set; }

        public ICommand DbCommand { get; private set; } 
        public ICommand HelpCommand { get; private set; } 
        public ICommand RedirectCommand { get; private set; } 
        public ICommand AppCommand { get; private set; } 

        public Window SettingsView { get; set; }
        public UserControl Menu { get; set; }

        public EnumEncoder EnumEncoder {get; private set; } = new EnumEncoder();  
        public EnumDecoder EnumDecoder {get; private set; } = new EnumDecoder(); 

        public Translator Translator { get; private set; }  

        private SaveFileDialog sfd = new SaveFileDialog();
        private OpenFileDialog ofd = new OpenFileDialog(); 

        public DataTable ResultCollection { get; private set; }
        public List<string> TablesCollection { get; private set; }

        public string RootFolder { get; private set; } = System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\.."; 
        
        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";
        
        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            string rootFolder = this.RootFolder; 
            this.Config = new Config(this, rootFolder); 

            this.AppDbConnection = new SqliteDbConnection($"{RootFolder}\\data\\app.db"); 
            
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 
            this.RedirectCommand = new RedirectCommand(this); 
            this.AppCommand = new AppCommand(this); 

            var appDbConnection = this.AppDbConnection; 
            this.Translator = new Translator(this); 
            this.Translator.SetAppDbConnection(appDbConnection); 
        }

        #region User DB methods 
        private void InitUserDbConnection()
        {
            try
            {
                if (AppRepository == null)
                {
                    throw new System.Exception("AppRepository is not assigned."); 
                }

                if (AppRepository.ActiveRdbms == RdbmsEnum.SQLite)
                {
                    // Unable to set an empty path for UserDbConnection, so it's better to assign UserDbConnection as null.  
                    this.UserDbConnection = null; 
                }
                else if (AppRepository.ActiveRdbms == RdbmsEnum.PostgreSQL)
                {
                    this.UserDbConnection = new PgDbConnection(); 
                }
                else if (AppRepository.ActiveRdbms == RdbmsEnum.MySQL)
                {
                    this.UserDbConnection = new MysqlDbConnection(); 
                }
                else 
                {
                    throw new System.Exception($"Unable to assign UserDbConnection, incorrect ActiveRdbms: {AppRepository.ActiveRdbms}."); 
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            finally
            {
                this.MainWindow.SqlPage.tblSqlPagePath.Text = string.Empty;

                var emptyDt = new DataTable(); 
                this.MainWindow.TablesPage.tvTables.Items.Clear();
                this.MainWindow.TablesPage.tblTablesPagePath.Text = string.Empty;
                this.MainWindow.TablesPage.dgrAllData.ItemsSource = emptyDt.DefaultView;
                this.MainWindow.TablesPage.dgrColumns.ItemsSource = emptyDt.DefaultView;
                this.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = emptyDt.DefaultView;
                this.MainWindow.TablesPage.dgrTriggers.ItemsSource = emptyDt.DefaultView;
                this.MainWindow.TablesPage.tbTableName.Text = string.Empty; 
                this.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
            }
        }

        private void InitLocalDbConnection(string path)
        {
            try
            {
                this.UserDbConnection = new SqliteDbConnection(path);
                this.MainWindow.SqlPage.tblSqlPagePath.Text = path;
                this.MainWindow.TablesPage.tblTablesPagePath.Text = path;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void SendSqlRequest()
        {
            try
            {
                if (this.UserDbConnection == null)
                {
                    throw new System.Exception("Database is not opened."); 
                }

                ResultCollection = this.UserDbConnection.ExecuteSqlCommand(this.MainWindow.SqlPage.mtbSqlRequest.Text);
                this.MainWindow.SqlPage.dbgSqlResult.ItemsSource = ResultCollection.DefaultView;

                this.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
                this.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        private void DisplayTablesInDb()
        {
            if (this.UserDbConnection == null)
            {
                return; 
            }

            try
            {
                string sqlRequest = GetSqlRequest("TableInfo\\DisplayTablesInDb.sql"); 
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.tvTables.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    TreeViewItem item = new TreeViewItem(); 
                    item.Header = row["name"].ToString();
                    this.MainWindow.TablesPage.tvTables.Items.Add(item); 
                }
                this.MainWindow.TablesPage.tvTables.IsEnabled = true; 
                this.MainWindow.TablesPage.tvTables.Visibility = Visibility.Visible; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 

            }
        }

        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrAllData.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA table_info({tableName});"; 
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrColumns.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA foreign_key_list('{tableName}');";
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrTriggers.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(GetSqlRequest("TableInfo\\GetSqlDefinition.sql"), tableName);
                DataTable dt = this.UserDbConnection.ExecuteSqlCommand(sqlRequest);
                if (dt.Rows.Count > 0) 
                {
                    DataRow row = dt.Rows[0];
                    this.MainWindow.TablesPage.mtbSqlTableDefinition.Text = row["sql"].ToString();
                }
                else 
                {
                    this.MainWindow.TablesPage.mtbSqlTableDefinition.Text = string.Empty;
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public string GetSqlRequest(string filename)
        {
            string sqlRequest = string.Empty; 
            try
            {
                sqlRequest = System.IO.File.ReadAllText($"{RootFolder}\\src\\Queries\\{filename}"); 
            }
            catch (System.Exception e) 
            {
                throw e; 
            }
            return sqlRequest; 
        }

        public void CreateLocalDb()
        {
            try
            {
                sfd.Filter = filter; 
                if (sfd.ShowDialog() == true)
                {
                    System.IO.File.WriteAllText(sfd.FileName, string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenLocalDb()
        {
            try
            {
                ofd.Filter = filter;
                if (ofd.ShowDialog() == true) {}

                string path = ofd.FileName; 
                if (path == string.Empty)
                {
                    return; 
                }
                InitLocalDbConnection(path); 
                DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // User DB methods 

        #region System DB methods
        public DataTable SendSqlRequest(string sql)
        {
            DataTable dt = new DataTable(); 
            try
            {
                if (this.AppDbConnection == null)
                {
                    throw new System.Exception("System RDBMS is not assigned."); 
                }
                dt = this.AppDbConnection.ExecuteSqlCommand(sql);
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            return dt; 
        }

        private void ClearTempTable(string tableName)
        {
            string sqlRequest = $"DELETE FROM {tableName};";
            try
            {
                this.AppDbConnection.ExecuteSqlCommand(sqlRequest);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion  // System DB methods
        
        #region Common UI methods 
        private void InitUI()
        {
            ((SettingsView)SettingsView).Init(); 

            ((UserControlsMenu)Menu).Init(); 
        }
        #endregion  // Common UI methods 

        #region Views methods
        public void OpenView(string viewName)
        {
            try
            {
                var type = System.Type.GetType("SqlViewer.Views." + viewName); 
                var win = System.Activator.CreateInstance(type) as System.Windows.Window; 
                win.DataContext = this;
                win.Show();
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void OpenDocsInBrowser(string docName, string title, string filePath)
        {
            string msg = "Do you want to open " + docName + " in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = filePath;
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }
        #endregion  // Views methods

        #region Pages methods
        public void RedirectToSqlQuery()
        {
            HideAllPages();
            DisableAllPages();

            this.MainWindow.SqlPage.Visibility = Visibility.Visible; 
            this.MainWindow.SqlPage.IsEnabled = true; 
        }

        public void RedirectToTables()
        {
            DisplayTablesInDb();

            HideAllPages();
            DisableAllPages();

            this.MainWindow.TablesPage.Visibility = Visibility.Visible; 
            this.MainWindow.TablesPage.IsEnabled = true; 
        }

        private void HideAllPages()
        {
            this.MainWindow.SqlPage.Visibility = Visibility.Collapsed; 
            this.MainWindow.TablesPage.Visibility = Visibility.Collapsed; 
        }

        private void DisableAllPages()
        {
            this.MainWindow.TablesPage.IsEnabled = false; 
            this.MainWindow.TablesPage.IsEnabled = false; 
        }
        #endregion  // Pages methods

        #region Application methods 
        public void InitAppRepository()
        {
            try
            {
                string sql = GetSqlRequest("App/SelectFromSettings.sql"); 
                DataTable dt = SendSqlRequest(sql); 

                string language = dt.Rows[0]["language"].ToString();
                string autoSave = dt.Rows[0]["auto_save"].ToString();
                int fontSize = System.Convert.ToInt32(dt.Rows[0]["font_size"]);
                string fontFamily = dt.Rows[0]["font_family"].ToString();
                int tabSize = System.Convert.ToInt32(dt.Rows[0]["tab_size"]);
                string wordWrap = dt.Rows[0]["word_wrap"].ToString();
                string defaultRdbms = dt.Rows[0]["default_rdbms"].ToString();
                string activeRdbms = dt.Rows[0]["active_rdbms"].ToString();
                string dbName = dt.Rows[0]["db_name"].ToString();
                string schemaName = dt.Rows[0]["schema_name"].ToString();
                string dbUsername = dt.Rows[0]["db_username"].ToString();
                string dbPswd = dt.Rows[0]["db_pswd"].ToString();

                var enumEncoder = this.EnumEncoder; 
                this.AppRepository = new AppRepository(enumEncoder, language, autoSave, 
                    fontSize, fontFamily, tabSize, wordWrap, defaultRdbms, activeRdbms, 
                    dbName, schemaName, dbUsername, dbPswd); 
                InitUserDbConnection(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Translate()
        {
            try
            {
                var language = AppRepository.Language; 
                this.Translator.SetLanguageEnum(language); 
                this.Translator.TranslateMenu(); 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RecoverSettings()
        {
            string msg = "Are you sure to recover settings changes?"; 
            if (System.Windows.MessageBox.Show(msg, "Recover settings", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ClearTempTable("tmp_settings");
                
                string sql = GetSqlRequest("App/RecoverSettings.sql"); 
                SendSqlRequest(sql); 
                InitAppRepository(); 
                Translate(); 
                InitUI(); 

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
                    ClearTempTable("tmp_settings"); 
                    ((SettingsView)SettingsView).UpdateAppRepository(); 

                    string sql = GetSqlRequest("App/UpdateSettingsEditor.sql"); 
                    sql = string.Format(sql, AppRepository.Language, AppRepository.AutoSave, 
                        EnumDecoder.GetFontSizeName(AppRepository.FontSize), AppRepository.FontFamily, 
                        EnumDecoder.GetTabSizeName(AppRepository.TabSize), AppRepository.WordWrap); 
                    SendSqlRequest(sql); 

                    sql = GetSqlRequest("App/UpdateSettingsDb.sql"); 
                    sql = string.Format(sql, AppRepository.DefaultRdbms, AppRepository.ActiveRdbms, 
                        AppRepository.DbName, AppRepository.DbSchema, AppRepository.DbUsername, 
                        AppRepository.DbPassword); 
                    SendSqlRequest(sql); 

                    InitAppRepository(); 
                    Translate(); 
                    InitUI(); 
                    System.Windows.MessageBox.Show("Settings saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                    SettingsView.Close(); 
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
                ClearTempTable("tmp_settings"); 
                System.Windows.MessageBox.Show("Settings cancelled", "Information", MessageBoxButton.OK, MessageBoxImage.Information); 
                SettingsView.Close(); 
            }
        }

        public void ExitApplication()
        {
            string msg = "Are you sure to close the application?"; 
            if (System.Windows.MessageBox.Show(msg, "Exit the application", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        #endregion  // Application methods 
    }
}