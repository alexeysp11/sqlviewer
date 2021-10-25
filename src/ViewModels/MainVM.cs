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

        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";

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
                string sqlRequest = System.IO.File.ReadAllText("src/SQL/ShowTablesInDb.sql"); 
                DataTable resultCollection = SqliteDbHelper.Instance.ExecuteSqlCommand(sqlRequest);

                this.MainWindow.tvTalbes.Items.Clear();

                foreach (DataRow row in resultCollection.Rows)
                {
                    TreeViewItem item = new TreeViewItem(); 
                    item.Header = row["name"].ToString();
                    this.MainWindow.tvTalbes.Items.Add(item); 
                }

                this.MainWindow.tvTalbes.IsEnabled = true; 
                this.MainWindow.tvTalbes.Visibility = Visibility.Visible; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception"); 
            }
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
                System.Windows.MessageBox.Show($"{ex.Message}", "Exception");
            }
        }

        public void OpenExistingDb()
        {
            try
            {
                ofd.Filter = filter;
                if (ofd.ShowDialog() == true)
                {
                    
                }

                string path = ofd.FileName; 
                SqliteDbHelper.Instance.SetPathToDb(path);
                this.MainWindow.tblSqlQueryGridPath.Text = path;
                this.MainWindow.tblTablesGridPath.Text = path;

                DisplayTablesInDb();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}", "Exception");
            }
        }

        public void ShowHelpWindow()
        {
            string msg = "Do you want to open documentation in your browser?"; 
            if (System.Windows.MessageBox.Show(msg, "Help", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                try 
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = "docs\\About.html";
                    process.Start();
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"{e.Message}", "Exception"); 
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