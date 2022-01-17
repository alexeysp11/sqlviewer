using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Documents; 
using System.Windows.Input; 
using Microsoft.Win32;
using SqlViewer.Commands; 
using SqlViewer.Models; 
using SqlViewer.Models.DbConnections; 
using SqlViewer.Views; 

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        private MainWindow MainWindow; 

        public Config Config { get; private set; } 

        public SqliteDbConnection AppDbConnection { get; private set; }
        public SqlViewer.Models.DbConnections.IDbConnection UserDbConnection { get; private set; }

        public ICommand DbCommand { get; private set; } 
        public ICommand HelpCommand { get; private set; } 
        public ICommand RedirectCommand { get; private set; } 
        public ICommand AppCommand { get; private set; } 

        private SaveFileDialog sfd = new SaveFileDialog();
        private OpenFileDialog ofd = new OpenFileDialog(); 

        public DataTable ResultCollection { get; private set; }
        public List<string> TablesCollection { get; private set; }

        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";
        private string RootFolder = System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\.."; 
        
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
                sqlRequest = System.IO.File.ReadAllText($"{RootFolder}\\src\\SQL\\{filename}"); 
            }
            catch (System.Exception e) 
            {
                throw e; 
            }
            return sqlRequest; 
        }

        public void CreateNewDb()
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

        public void OpenExistingDb()
        {
            try
            {
                ofd.Filter = filter;
                if (ofd.ShowDialog() == true) {}

                string path = ofd.FileName; 
                this.UserDbConnection = new SqliteDbConnection(path);
                this.MainWindow.SqlPage.tblSqlPagePath.Text = path;
                this.MainWindow.TablesPage.tblTablesPagePath.Text = path;

                DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowUserGuide(string filename)
        {
            string msg = "Do you want to open documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "User's guide", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = $"{RootFolder}\\docs\\{filename}.html";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }

        public void ShowSqliteDocs()
        {
            string msg = "Do you want to open SQLite documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "Common SQL docs", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = $"https://www.sqlite.org/index.html";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }

        public void ShowPosgresDocs()
        {
            string msg = "Do you want to open PostgreSQL documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "Common SQL docs", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = $"https://www.postgresql.org/";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }

        public void ShowMySqlDocs()
        {
            string msg = "Do you want to open MySQL documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "Common SQL docs", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = $"https://dev.mysql.com/doc/";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
                }
            }
        }

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

        public void OpenSettingsWindow()
        {
            try
            {
                var win = new SettingsWindow();
                win.DataContext = this;
                win.Show();
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void OpenOptionsWindow()
        {
            try
            {
                var win = new OptionsWindow();
                win.DataContext = this;
                win.Show();
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        public void OpenConnectionsWindow()
        {
            try
            {
                var win = new ConnectionsWindow();
                win.DataContext = this;
                win.Show();
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
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