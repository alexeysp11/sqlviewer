using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Documents; 
using System.Windows.Input; 
using Microsoft.Win32;
using SqlViewer.Commands; 
using SqlViewer.Models.Database; 
using SqlViewer.Views; 

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        private MainWindow MainWindow; 

        public ICommand SendSqlCommand { get; private set; } 
        public ICommand DbCommand { get; private set; } 
        public ICommand HelpCommand { get; private set; } 
        public ICommand RedirectCommand { get; private set; } 

        private SaveFileDialog sfd = new SaveFileDialog();
        private OpenFileDialog ofd = new OpenFileDialog(); 

        public DataTable ResultCollection { get; private set; }
        public List<string> TablesCollection { get; private set; }

        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";

        private string[] PathsToSqlFolder = new string[] { "src/SQL/", "../../../SQL/" }; 
        
        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.SendSqlCommand = new SendSqlCommand(this); 
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 
            this.RedirectCommand = new RedirectCommand(this); 
        }

        public void SendSqlRequest()
        {
            try
            {
                ResultCollection = SqliteDbConnection.Instance.ExecuteSqlCommand(this.MainWindow.SqlPage.mtbSqlRequest.Text);
                this.MainWindow.SqlPage.dbgSqlResult.ItemsSource = ResultCollection.DefaultView;

                this.MainWindow.SqlPage.dbgSqlResult.Visibility = Visibility.Visible; 
                this.MainWindow.SqlPage.dbgSqlResult.IsEnabled = true; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        private void DisplayTablesInDb()
        {
            try
            {
                string sqlRequest = GetSqlRequest("DisplayTablesInDb.sql"); 
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
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
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        public void GetAllDataFromTable(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM {tableName}"; 
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrAllData.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        public void GetColumnsOfTable(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA table_info({tableName});"; 
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrColumns.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        public void GetForeignKeys(string tableName)
        {
            try
            {
                string sqlRequest = $"PRAGMA foreign_key_list('{tableName}');";
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrForeignKeys.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        public void GetTriggers(string tableName)
        {
            try
            {
                string sqlRequest = $"SELECT * FROM sqlite_master WHERE type = 'trigger' AND tbl_name LIKE '{tableName}';";
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.TablesPage.dgrTriggers.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        public void GetSqlDefinition(string tableName)
        {
            try
            {
                string sqlRequest = string.Format(GetSqlRequest("GetSqlDefinition.sql"), tableName);
                DataTable dt = SqliteDbConnection.Instance.ExecuteSqlCommand(sqlRequest);
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
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
        }

        private string GetSqlRequest(string filename)
        {
            string sqlRequest = string.Empty; 
            foreach (string path in PathsToSqlFolder)
            {
                try
                {
                    sqlRequest = System.IO.File.ReadAllText(path + filename); 
                    break; 
                }
                catch (System.Exception) {}
            }
            if (sqlRequest == string.Empty)
            {
                throw new System.Exception("Unable to display tables located in the database"); 
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
                System.Windows.MessageBox.Show(ex.Message, "Exception");
            }
        }

        public void OpenExistingDb()
        {
            try
            {
                ofd.Filter = filter;
                if (ofd.ShowDialog() == true) {}

                string path = ofd.FileName; 
                SqliteDbConnection.Instance.SetPathToDb(path);
                this.MainWindow.SqlPage.tblSqlPagePath.Text = path;
                this.MainWindow.TablesPage.tblTablesPagePath.Text = path;

                DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Exception");
            }
        }

        public void ShowUserGuide(string filename)
        {
            string msg = "Do you want to open documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "User's guide", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = $"docs\\{filename}.html";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Exception"); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
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
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
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
    }
}