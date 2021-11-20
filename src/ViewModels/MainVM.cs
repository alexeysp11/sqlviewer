using System.Collections.Generic; 
using System.Data; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Documents; 
using System.Windows.Input; 
using Microsoft.Win32;
using SqlViewer.Commands; 
using SqlViewer.Models.Database; 
using SqlViewer.View; 

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

            this.MainWindow.ResultDataGrid.Visibility = Visibility.Collapsed; 
            this.MainWindow.ResultDataGrid.IsEnabled = false;
        }

        public void SendSqlRequest()
        {
            try
            {
                ResultCollection = SqliteDbHelper.Instance.ExecuteSqlCommand(this.MainWindow.tbMultiLine.Text);
                this.MainWindow.ResultDataGrid.ItemsSource = ResultCollection.DefaultView;

                this.MainWindow.ResultDataGrid.Visibility = Visibility.Visible; 
                this.MainWindow.ResultDataGrid.IsEnabled = true; 
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.tvTables.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    TreeViewItem item = new TreeViewItem(); 
                    item.Header = row["name"].ToString();
                    this.MainWindow.tvTables.Items.Add(item); 
                }
                this.MainWindow.tvTables.IsEnabled = true; 
                this.MainWindow.tvTables.Visibility = Visibility.Visible; 
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.dgrAllData.ItemsSource = dt.DefaultView;
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.dgrColumns.ItemsSource = dt.DefaultView;
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.dgrForeignKeys.ItemsSource = dt.DefaultView;
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                this.MainWindow.dgrTriggers.ItemsSource = dt.DefaultView;
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
                DataTable dt = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);
                if (dt.Rows.Count > 0) 
                {
                    DataRow row = dt.Rows[0];
                    this.MainWindow.tbSqlTableDefinition.Text = row["sql"].ToString();
                }
                else 
                {
                    this.MainWindow.tbSqlTableDefinition.Text = string.Empty;
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
                SqliteDbHelper.Instance.SetPathToDb(path);
                this.MainWindow.tblSqlQueryGridPath.Text = path;
                this.MainWindow.tblTablesGridPath.Text = path;

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

            this.MainWindow.SqlQueryGrid.Visibility = Visibility.Visible; 
            this.MainWindow.SqlQueryGrid.IsEnabled = true; 
        }

        public void RedirectToTables()
        {
            DisplayTablesInDb();

            HideAllPages();
            DisableAllPages();

            this.MainWindow.TablesGrid.Visibility = Visibility.Visible; 
            this.MainWindow.TablesGrid.IsEnabled = true; 
        }

        private void HideAllPages()
        {
            this.MainWindow.SqlQueryGrid.Visibility = Visibility.Collapsed; 
            this.MainWindow.TablesGrid.Visibility = Visibility.Collapsed; 
        }

        private void DisableAllPages()
        {
            this.MainWindow.TablesGrid.IsEnabled = false; 
            this.MainWindow.TablesGrid.IsEnabled = false; 
        }
    }
}