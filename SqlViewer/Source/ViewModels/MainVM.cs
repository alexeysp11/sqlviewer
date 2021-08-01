using System.Windows.Input; 
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

        private const string filter = "All files|*.*|Database files|*.db|SQLite3 files|*.sqlite3";

        public MainVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.SendSqlCommand = new SendSqlCommand(this); 
            this.DbCommand = new DbCommand(this); 
            this.HelpCommand = new HelpCommand(this); 
        }

        public void SendSqlRequest()
        {
            try
            {
                this.MainWindow.richTextBox1.Document.Blocks.Clear();
                
                SqliteDbHelper.Instance.SetPathToDb("C:\\Users\\User\\Desktop\\projects\\sqlviewer\\TestDatabases\\TestDb.sqlite3");
                string result = SqliteDbHelper.Instance.ExecuteSqlCommand(this.MainWindow.tbMultiLine.Text);
                
                this.MainWindow.richTextBox1.Document.Blocks.Add(new Paragraph(new Run($"{result}"))); 
            }
            catch (System.Exception e)
            {
                this.MainWindow.richTextBox1.Document.Blocks.Add(new Paragraph(new Run($"Exception: {e}")));
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