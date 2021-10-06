using System.Data; 
using System.Windows.Input; 
using System.Windows;
using System.Windows.Documents; 
using Microsoft.Win32;
using SqlViewer.View; 
using SqlViewer.Commands; 
using SqlViewer.Models.Database; 

namespace SqlViewer.ViewModels
{
    public class MainVM
    {
        private MainWindow MainWindow; 

        public ICommand SendSqlCommand { get; private set; } 
        public ICommand DbCommand { get; private set; } 
        public ICommand HelpCommand { get; private set; } 

        private SaveFileDialog sfd = new SaveFileDialog();
        private OpenFileDialog ofd = new OpenFileDialog(); 

        public DataTable ResultCollection { get; private set; }

        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";

        private string path = "C:\\Users\\User\\Desktop\\projects\\sqlviewer\\Databases\\TestDb.sqlite3";

        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.SendSqlCommand = new SendSqlCommand(this); 
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 

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
                System.Windows.MessageBox.Show($"Exception: {ex}", "Exception");
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

                path = ofd.FileName; 
                SqliteDbHelper.Instance.SetPathToDb(path);
                this.MainWindow.PathTextBlock.Text = path;
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Exception: {ex}", "Exception");
            }
        }

        public void ShowHelpWindow()
        {
            System.Windows.MessageBox.Show("ShowHelpWindow");
        }
    }
}